using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FimFictionHistoryBrowser.Annotations;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace FimFictionHistoryBrowser
{
    public class DataReader : INotifyPropertyChanged
    {
        private Story _selectedStory;
        private Story _newStory;
        private FimFictionDatabaseEntities _db;
        private ObservableCollection<string> _progressComboBoxCollection;
        private ObservableCollection<string> _statusComboCollection;
        private ObservableCollection<string> _audienceComboCollection;
        private ObservableCollection<int> _ratingComboCollection;
        private ObservableCollection<string> _downloadComboCollection;
        private ObservableCollection<Story> _stories;
        private ICommand _addStoryCommand;
        private ICommand _loadStoryDatabaseCommand;
        private ICommand _clearFormCommand;
        private ICommand _updateStoryCommand;
        private ICommand _removeStoryCommand;
        private ICommand _browseCommand;
        private ICollectionView _progressView;
        private ICollectionView _statusView;
        private ICollectionView _audienceView;
        private ICollectionView _ratingView;
        private ICollectionView _downloadView;
        private string _storyFileName;
        private ICommand _aboutCommand;

        public DataReader()
        {
            Version = "Version 0.7.1.0"; 

            Stories = new ObservableCollection<Story>();
            _db =  new FimFictionDatabaseEntities();
            _newStory = new Story();
            LoadStoryDatabase();
            _progressComboBoxCollection = new ObservableCollection<string>() { "Wait-List", "Finished", "Reading", "On-Pause" };
            _statusComboCollection = new ObservableCollection<string>() { "Incomplete", "Complete", "On-Hiatus" };
            _audienceComboCollection = new ObservableCollection<string>() { "Everyone", "Teen", "Teen, Sex", "Teen, Gore", "Teen, Sex, Gore", "Mature", "Mature, Sex", "Mature, Gore", "Mature, Sex, Gore" };
            _ratingComboCollection = new ObservableCollection<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            _downloadComboCollection = new ObservableCollection<string>() { "Dropbox", "-" };

            ProgressView = CollectionViewSource.GetDefaultView(ProgressComboBoxCollection);
            ProgressView.CurrentChanged += ProgressViewOnCurrentChanged;
            StatusView = CollectionViewSource.GetDefaultView(StatusComboCollection);
            StatusView.CurrentChanged += StatusViewOnCurrentChanged;
            AudienceView = CollectionViewSource.GetDefaultView(AudienceComboCollection);
            AudienceView.CurrentChanged += AudienceViewOnCurrentChanged;
            RatingView = CollectionViewSource.GetDefaultView(RatingComboCollection);
            RatingView.CurrentChanged += RatingViewOnCurrentChanged;
            DownloadView = CollectionViewSource.GetDefaultView(DownloadComboCollection);
            DownloadView.CurrentChanged += DownloadViewOnCurrentChanged;
        }

        #region ComboBox settings

        public ObservableCollection<string> ProgressComboBoxCollection
        {
            get { return _progressComboBoxCollection; }
            set
            {
                _progressComboBoxCollection = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView ProgressView
        {
            get { return _progressView; }
            set
            {
                if (Equals(value, _progressView)) return;
                _progressView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> StatusComboCollection
        {
            get { return _statusComboCollection; }
            set
            {
                _statusComboCollection = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView StatusView
        {
            get { return _statusView; }
            set
            {
                if (Equals(value, _statusView)) return;
                _statusView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> AudienceComboCollection
        {
            get { return _audienceComboCollection; }
            set
            {
                _audienceComboCollection = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView AudienceView
        {
            get { return _audienceView; }
            set
            {
                if (Equals(value, _audienceView)) return;
                _audienceView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> RatingComboCollection
        {
            get { return _ratingComboCollection; }
            set
            {
                _ratingComboCollection = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView RatingView
        {
            get { return _ratingView; }
            set
            {
                if (Equals(value, _ratingView)) return;
                _ratingView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> DownloadComboCollection
        {
            get { return _downloadComboCollection; }
            set
            {
                _downloadComboCollection = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView DownloadView
        {
            get { return _downloadView; }
            set
            {
                if (Equals(value, _downloadView)) return;
                _downloadView = value;
                OnPropertyChanged();
            }
        }

        private void DownloadViewOnCurrentChanged(object sender, EventArgs eventArgs)
        {
            if (DownloadView.CurrentItem == null) return;
            var selectedValue = DownloadView.CurrentItem as string;

            if (selectedValue == null) return;

            DownloadStatus = selectedValue;
        }

        private void RatingViewOnCurrentChanged(object sender, EventArgs eventArgs)
        {
            if (RatingView.CurrentItem == null) return;
            var selectedValue = RatingView.CurrentItem as int?;

            if (selectedValue == null) return;

            Rating = (int)selectedValue;
        }

        private void AudienceViewOnCurrentChanged(object sender, EventArgs eventArgs)
        {
            if (AudienceView.CurrentItem == null) return;
            var selectedValue = AudienceView.CurrentItem as string;

            if (selectedValue == null) return;

            Audience = selectedValue;
        }

        private void StatusViewOnCurrentChanged(object sender, EventArgs eventArgs)
        {
            if (StatusView.CurrentItem == null) return;
            var selectedValue = StatusView.CurrentItem as string;

            if (selectedValue == null) return;

            Status = selectedValue;
        }

        private void ProgressViewOnCurrentChanged(object sender, EventArgs eventArgs)
        {
            if (ProgressView.CurrentItem == null) return;
            var selectedValue = ProgressView.CurrentItem as string;

            if (selectedValue == null) return;

            Progress = selectedValue;
        }

        #endregion

        #region public properties and property methods

        public string StoryFileName
        {
            get { return _storyFileName; }
            set
            {
                if (value == _storyFileName) return;
                _storyFileName = value;
                OnPropertyChanged();
            }
        }

        public Story NewStory
        {
            get { return _newStory; }
            set
            {
                _newStory = value;
                SetComboBoxValuesOnChange(value);
                OnPropertyChanged("NewStory");
            }
        }

        public Story SelectedStory
        {
            get { return _selectedStory; }
            set
            {
                _selectedStory = value;
                NewStory = value;
            }
        }

        public string Version { get; set; }
        public string Progress { get; set; }
        public string Audience { get; set; }
        public string Status { get; set; }
        public string DownloadStatus { get; set; }
        public int Rating { get; set; }

        public ObservableCollection<Story> Stories
        {
            get { return _stories; }
            set
            {
                if (Equals(value, _stories)) return;
                _stories = value;
                OnPropertyChanged();
            }
        }

        private void SetComboBoxValuesOnChange(Story story)
        {
            if (story == null)
            {
                RatingView.MoveCurrentToFirst();
                ProgressView.MoveCurrentToFirst();
                AudienceView.MoveCurrentToFirst();
                StatusView.MoveCurrentToFirst();
                DownloadView.MoveCurrentToFirst();
                return;
            }
            RatingView.MoveCurrentTo(story.Rating);
            ProgressView.MoveCurrentTo(story.Progress);
            AudienceView.MoveCurrentTo(story.Audience);
            StatusView.MoveCurrentTo(story.Status);
            DownloadView.MoveCurrentTo(story.DownloadStatus);
        }

        #endregion

        #region Commands

        public ICommand AboutCommand
        {
            get { return _aboutCommand ?? (_aboutCommand = new RelayCommand(OpenAbout)); }
        }

        private void OpenAbout()
        {
            var about = new AboutBox();
            about.ShowDialog();
        }

        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(Browse)); }
        }

        private void Browse()
        {
            var browse = new OpenFileDialog()
            {
                Filter = "E-Books (*.epub)|*.EPUB|All files(*.*)|*.*",
                Title = "Locate the E-Book for the selected story.",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
            };

            if ( browse.ShowDialog() == true )
            {
                StoryFileName = browse.SafeFileName;
            }
        }

        public ICommand ClearFormCommand
        {
            get { return _clearFormCommand ?? (_clearFormCommand = new RelayCommand(ClearForm)); }
        }

        private void ClearForm()
        {
            NewStory = new Story();
        }

        public ICommand LoadStoryDatabaseCommand
        {
            get { return _loadStoryDatabaseCommand ?? (_loadStoryDatabaseCommand = new RelayCommand(LoadStoryDatabase)); }
        }

        public void LoadStoryDatabase()
        {
            Stories = new ObservableCollection<Story>();
            var story = from s in _db.Stories select s;
            
            foreach (var part in story)
            {
                Stories.Add(part);
            }
        }

        public ICommand AddStoryCommand
        {
            get { return _addStoryCommand ?? (_addStoryCommand = new RelayCommand(AddStory)); }
        }

        private void AddStory()
        {
            var story = new Story();

            story.Name = NewStory.Name;
            story.WordCount = NewStory.WordCount;
            story.ChapterCount = NewStory.ChapterCount;
            story.Tags = NewStory.Tags;
            story.Author = NewStory.Author;
            story.StoryLink = NewStory.StoryLink;
            story.Sequels = NewStory.Sequels;
            story.Series = NewStory.Series;
            story.Notes = NewStory.Notes;
            story.StorageEPUB = StoryFileName;

            story.Rating = Rating;
            story.Progress = Progress;
            story.Audience = Audience;
            story.DownloadStatus = DownloadStatus;
            story.Status = Status;
            
            _db.Stories.Add(story);

            try
            {
                _db.SaveChanges();

                LoadStoryDatabase();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        public ICommand UpdateStoryCommand
        {
            get { return _updateStoryCommand ?? (_updateStoryCommand = new RelayCommand(UpdateStory)); }
        }

        private void UpdateStory()
        {
            var story = new Story();

            story.Id = NewStory.Id;
            story.Name = NewStory.Name;
            story.WordCount = NewStory.WordCount;
            story.ChapterCount = NewStory.ChapterCount;
            story.Tags = NewStory.Tags;
            story.Author = NewStory.Author;
            story.StoryLink = NewStory.StoryLink;
            story.Sequels = NewStory.Sequels;
            story.Series = NewStory.Series;
            story.Notes = NewStory.Notes;
            story.StorageEPUB = StoryFileName;

            story.Rating = Rating;
            story.Progress = Progress;
            story.Audience = Audience;
            story.DownloadStatus = DownloadStatus;
            story.Status = Status;

            var storyToEdit = _db.Stories.Find(story.Id);
            if (storyToEdit != null)
            {
                _db.Entry(storyToEdit).CurrentValues.SetValues(story);

                try
                {
                    _db.SaveChanges();
                    LoadStoryDatabase();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
        }

        public ICommand RemoveStoryCommand { get
        {
            return _removeStoryCommand ?? (_removeStoryCommand = new RelayCommand(RemoveStory));
        } }

        private void RemoveStory()
        {
            MessageBoxResult result = MessageBox.Show("Are you sur eyou want to delete the selected story?",
                "Remove Story", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                _db.Stories.Remove(SelectedStory);
                try
                {
                    _db.SaveChanges();
                    LoadStoryDatabase();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}