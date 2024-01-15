using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis {
    public class SemanticModelCache {
        private readonly IDictionary<string, SemanticModel> modelCollection = new Dictionary<string, SemanticModel>();

        public async Task Init(Solution solution, CancellationToken cancellationToken = default) {
            await InitInternal(solution.Projects.SelectMany(e => e.Documents), cancellationToken);
        }

        public async Task Init(Project project, CancellationToken cancellationToken = default) {
            await InitInternal(project.Documents, cancellationToken);
        }

        private async Task InitInternal(IEnumerable<Document> documents, CancellationToken cancellationToken = default) {
            Dictionary<string, Task<SemanticModel>> taskMapping = GetTaskMapping(documents, cancellationToken);
            SemanticModel[] semanticModels = await Task.WhenAll(taskMapping.Values);

            Dictionary<string, Task<SemanticModel>>.Enumerator enumerator = taskMapping.GetEnumerator();
            int counter = 0;

            modelCollection.Add(enumerator.Current.Key, semanticModels[counter]);
            while (enumerator.MoveNext()) {
                counter++;
                modelCollection.Add(enumerator.Current.Key, semanticModels[counter]);
            }

            enumerator.Dispose();
        }

        private Dictionary<string, Task<SemanticModel>> GetTaskMapping(IEnumerable<Document> documents, CancellationToken cancellationToken = default) {
            Dictionary<string, Task<SemanticModel>> taskMapping = new Dictionary<string, Task<SemanticModel>>();
            foreach (Document document in documents) {
                if (document.FilePath == null || !document.SupportsSemanticModel || taskMapping.ContainsKey(document.FilePath))
                    continue;

                taskMapping.Add(document.FilePath, document.GetSemanticModelAsync(cancellationToken));
            }

            return taskMapping;
        }
    }
}
