﻿using System;
using JetBrains.Annotations;
using PostPublication;
using Xamarin.Forms.Xaml;

namespace Missio
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PublicationPage
	{
	    [Obsolete("Only for previewing with the Xamarin previewer", true)]
        public PublicationPage ()
		{
            App.AssertIsPreviewing();
			InitializeComponent ();
		}

        [UsedImplicitly]
	    public PublicationPage(PublicationPageViewModel publicationPageViewModel)
	    {
            BindingContext = publicationPageViewModel;
            InitializeComponent();
	    }
	}
}