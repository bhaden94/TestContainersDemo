using Demo.Function.Tests.Fixtures;

namespace Demo.Function.Tests.Collections;

[CollectionDefinition(nameof(FunctionCollection))]
public class FunctionCollection : ICollectionFixture<FunctionFixture>;
