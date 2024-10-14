using Demo.ApiToFunction.Tests.Fixtures;

namespace Demo.ApiToFunction.Tests.Collections;

[CollectionDefinition(nameof(FunctionCollection))]
public class FunctionCollection : ICollectionFixture<FunctionFixture>;
