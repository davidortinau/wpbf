using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.XForms.Buttons;
using WhitePaperBible.Core.Models;
using WhitePaperBible.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhitePaperBible.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPaperPage : ContentPage
    {
        public AddPaperPage()
        {
            InitializeComponent();
        }


        private void InputEntry_OnCompleted(object sender, EventArgs e)
        {
            (BindingContext as AddPaperViewModel).CompletedCommand.Execute(InputEntry.Text);
            InputEntry.Text = "";
        }

        private void SfChipGroup_OnChipClicked(object sender, EventArgs e)
        {
            (BindingContext as AddPaperViewModel).RemoveTagCommand.Execute((sender as SfChip).Text);
        }

        private void VerseInputEntry_OnCompleted(object sender, EventArgs e)
        {
            (BindingContext as AddPaperViewModel).AddReferenceCommand.Execute((sender as Entry).Text);
            VerseInputEntry.Text = "";
        }

        private void Verse_OnChipClicked(object sender, EventArgs e)
        {
            (BindingContext as AddPaperViewModel).RemoveReferenceCommand.Execute((sender as SfChip).Text);
        }
    }
    
    public class SfPickerBehavior:Behavior<Syncfusion.SfPicker.XForms.SfPicker>
    {
        protected override void OnAttachedTo(Syncfusion.SfPicker.XForms.SfPicker bindable)
        {
            base.OnAttachedTo(bindable);
        }
        protected override void OnDetachingFrom(Syncfusion.SfPicker.XForms.SfPicker bindable)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                (bindable as Syncfusion.SfPicker.XForms.SfPicker).Dispose();
            }
            base.OnDetachingFrom(bindable);
        }
    }
}