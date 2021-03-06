using FubuCore;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using ripple.Model;
using ripple.Nuget;

namespace ripple.Testing.Nuget
{
	[TestFixture]
	public class NugetFolderCacheTester : InteractionContext<NugetFolderCache>
	{
		protected override void beforeEach()
		{
			Services.Inject("");
			Services.PartialMockTheClassUnderTest();
		}

		[Test]
		public void no_local_file_returns_a_wrapped_nuget()
		{
			var theDependency = new Dependency("Bottles", "1.0.0.0");
			var theNuget = new StubNuget(theDependency);

			ClassUnderTest.Stub(x => x.Find(theDependency)).Return(null);

			var nuget = ClassUnderTest.Retrieve(theNuget).As<CacheableNuget>();
			nuget.Inner.ShouldBeTheSameAs(theNuget);
		}

		[Test]
		public void local_file_returns_a_cached_nuget()
		{
			var theDependency = new Dependency("Bottles", "1.0.0.0");
			var theNuget = new StubNuget(theDependency);
			var theLocalFile = MockFor<INugetFile>();

			ClassUnderTest.Stub(x => x.Find(theDependency)).Return(theLocalFile);

			var cached = ClassUnderTest.Retrieve(theNuget) as FileSystemNuget;
			cached.File.ShouldBeTheSameAs(theLocalFile);
		}

		[Test]
		public void cached_file_is_returned()
		{
			var cached = new FileSystemNuget(MockFor<INugetFile>());
			ClassUnderTest.Retrieve(cached).ShouldBeTheSameAs(cached);
		}
	}
}