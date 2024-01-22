using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HBLibrary.Code.Analysis; 
public class SemanticModelCache {
    private readonly IDictionary<string, SemanticModel> modelCollection = new Dictionary<string, SemanticModel>();

    public static async Task<SemanticModelCache> FromSolutionAsync(Solution solution, CancellationToken cancellationToken = default) {
        SemanticModelCache modelCache = new SemanticModelCache();
        await modelCache.InitAsync(solution.Projects.SelectMany(e => e.Documents), cancellationToken);
        return modelCache;
    }

    public static async Task<SemanticModelCache> FromProjectAsync(Project project, CancellationToken cancellationToken = default) {
        SemanticModelCache modelCache = new SemanticModelCache();
        await modelCache.InitAsync(project.Documents, cancellationToken);
        return modelCache;
    }

    public static async Task<SemanticModelCache> FromDocumentsAsync(IEnumerable<Document> documents, CancellationToken cancellationToken = default) {
        SemanticModelCache modelCache = new SemanticModelCache();
        await modelCache.InitAsync(documents, cancellationToken);
        return modelCache;
    }

    private async Task InitAsync(IEnumerable<Document> documents, CancellationToken cancellationToken = default) {
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
