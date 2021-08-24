﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer
{
    internal class DefaultDocumentVersionCache : DocumentVersionCache
    {
        internal const int MaxDocumentTrackingCount = 20;

        // Internal for testing
        internal readonly Dictionary<string, List<DocumentEntry>> DocumentLookup;
        private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
        private ProjectSnapshotManagerBase _projectSnapshotManager;

        public DefaultDocumentVersionCache(ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher)
        {
            if (projectSnapshotManagerDispatcher == null)
            {
                throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
            }

            DocumentLookup = new Dictionary<string, List<DocumentEntry>>(FilePathComparer.Instance);
            _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        }

        public override void TrackDocumentVersion(DocumentSnapshot documentSnapshot, int version)
        {
            if (documentSnapshot == null)
            {
                throw new ArgumentNullException(nameof(documentSnapshot));
            }

            _projectSnapshotManagerDispatcher.AssertDispatcherThread();

            if (!DocumentLookup.TryGetValue(documentSnapshot.FilePath, out var documentEntries))
            {
                documentEntries = new List<DocumentEntry>();
                DocumentLookup[documentSnapshot.FilePath] = documentEntries;
            }

            if (documentEntries.Count == MaxDocumentTrackingCount)
            {
                // Clear the oldest document entry

                // With this approach we'll slowly leak memory as new documents are added to the system. We don't clear up
                // document file paths where where all of the corresponding entries are expired.
                documentEntries.RemoveAt(0);
            }

            var entry = new DocumentEntry(documentSnapshot, version);
            documentEntries.Add(entry);
        }

        public override bool TryGetDocumentVersion(DocumentSnapshot documentSnapshot, out int? version)
        {
            if (documentSnapshot == null)
            {
                throw new ArgumentNullException(nameof(documentSnapshot));
            }

            _projectSnapshotManagerDispatcher.AssertDispatcherThread();

            if (!DocumentLookup.TryGetValue(documentSnapshot.FilePath, out var documentEntries))
            {
                version = null;
                return false;
            }

            DocumentEntry entry = null;
            for (var i = documentEntries.Count - 1; i >= 0; i--)
            {
                // We iterate backwards over the entries to prioritize newer entries.
                if (documentEntries[i].Document.TryGetTarget(out var document) &&
                    document == documentSnapshot)
                {
                    entry = documentEntries[i];
                    break;
                }
            }

            if (entry == null)
            {
                version = null;
                return false;
            }

            version = entry.Version;
            return true;
        }

        public override void Initialize(ProjectSnapshotManagerBase projectManager)
        {
            _projectSnapshotManager = projectManager;
            _projectSnapshotManager.Changed += ProjectSnapshotManager_Changed;
        }

        private void ProjectSnapshotManager_Changed(object sender, ProjectChangeEventArgs args)
        {
            // Don't do any work if the solution is closing
            if (args.SolutionIsClosing)
            {
                return;
            }

            _projectSnapshotManagerDispatcher.AssertDispatcherThread();

            switch (args.Kind)
            {
                case ProjectChangeKind.DocumentChanged:
                    if (DocumentLookup.ContainsKey(args.DocumentFilePath) &&
                        !_projectSnapshotManager.IsDocumentOpen(args.DocumentFilePath))
                    {
                        // Document closed, evict entry.
                        DocumentLookup.Remove(args.DocumentFilePath);
                    }
                    break;
            }

            // Any event that has a project may have changed the state of the documents
            // and therefore requires us to mark all existing documents as latest.
            if (args.ProjectFilePath == null)
            {
                return;
            }

            var project = _projectSnapshotManager.GetLoadedProject(args.ProjectFilePath);
            if (project == null)
            {
                // Project no longer loaded, wait for document removed event.
                return;
            }

            CaptureProjectDocumentsAsLatest(project);
        }

        // Internal for testing
        internal void MarkAsLatestVersion(DocumentSnapshot document)
        {
            if (!TryGetLatestVersionFromPath(document.FilePath, out var latestVersion))
            {
                return;
            }

            // Update our internal tracking state to track the changed document as the latest document.
            TrackDocumentVersion(document, latestVersion.Value);
        }

        // Internal for testing
        internal bool TryGetLatestVersionFromPath(string filePath, out int? version)
        {
            if (!DocumentLookup.TryGetValue(filePath, out var documentEntries))
            {
                version = null;
                return false;
            }

            var latestEntry = documentEntries[documentEntries.Count - 1];

            version = latestEntry.Version;
            return true;
        }

        private void CaptureProjectDocumentsAsLatest(ProjectSnapshot projectSnapshot)
        {
            foreach (var documentPath in projectSnapshot.DocumentFilePaths)
            {
                if (DocumentLookup.ContainsKey(documentPath))
                {
                    var document = projectSnapshot.GetDocument(documentPath);
                    MarkAsLatestVersion(document);
                }
            }
        }

        internal class DocumentEntry
        {
            public DocumentEntry(DocumentSnapshot document, int version)
            {
                Document = new WeakReference<DocumentSnapshot>(document);
                Version = version;
            }

            public WeakReference<DocumentSnapshot> Document { get; }

            public int Version { get; }
        }
    }
}
