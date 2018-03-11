﻿using Xamarin.Forms;
using Mission.Model.LocalProviders;
using Ninject;
using Ninject.Modules;
using ViewModel;

namespace Missio
{
	public partial class App
	{
		public App ()
		{
            var kernel = new KernelConfiguration(new ModelModule(), new ViewModelModule(), new NewsFeedModule(), new LogInModule()).BuildReadonlyKernel();
            InitializeComponent();
            MainPage = new NavigationPage(kernel.Get<LogIn>());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
	
	public class ViewModelModule : NinjectModule
	{
		/// <inheritdoc />
		public override void Load()
		{
			Bind<IDisplayAlertOnCurrentPage>().To<DisplayAlertOnCurrentPage>().InSingletonScope();
			Bind<IOnUserLoggedIn, GlobalUser>().To<GlobalUser>().InSingletonScope();
            Bind<IAttemptToLogin>().To<AttemptToLogIn>().InSingletonScope();
        }
	}

    public class NewsFeedModule : NinjectModule
    {
        /// <inheritdoc />
        public override void Load()
        {
            Bind<NewsFeedViewModel>().ToSelf().InSingletonScope();
            Bind<NewsFeedPage>().ToSelf().InSingletonScope();
            Bind<IGoToNextPage>().To<GoToPage>().Named("GoToNewsFeed").WithConstructorArgument("page", x => x.Kernel.Get<NewsFeedPage>());
        }
    }

	public class LogInModule : NinjectModule
	{
		/// <inheritdoc />
		public override void Load()
		{
			Bind<AttemptToLogIn>().ToSelf().InSingletonScope();
            Bind<LogInViewModel>().ToSelf().InSingletonScope();
            Bind<LogIn>().ToSelf().InSingletonScope();
		}
	}

    public class ModelModule : NinjectModule
    {
        /// <inheritdoc />
        public override void Load()
        {
#if USE_FAKE_DATA
            Bind<INewsFeedPostsProvider>().To<FakeNewsFeedPostProvider>().InSingletonScope();
            Bind<IUserValidator>().To<FakeUserValidator>().InSingletonScope();
            Bind<INewsFeedPostsUpdater>().To<FakeNewsFeedPostsUpdater>().InSingletonScope();
#else
#endif
        }
    }
}
