using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.FrameNavigationService;
public interface IFrameNavigationBuilder {
    IFrameNavigationBuilder Add(string frameKey, Func<IFrameNavigationModelBuilder, FrameNavigationModel> frameNavigationModelBuilder);

    Dictionary<string, FrameNavigationModel> Build();
}

public interface IFrameNavigationModelBuilder {
    IFrameNavigationModelBuilder Add(Uri uri, Type type);
    IFrameNavigationModelBuilder AddBasePath(string basePath);

    FrameNavigationModel Build();
}
