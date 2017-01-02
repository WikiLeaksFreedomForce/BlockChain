using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using BlockChain.WPF.Data;
using BlockChain.WPF.Dialogs;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;
using BlockChain.WPF.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace BlockChain.WPF.ViewModels {

    public class MainWindowViewModel : ViewModelBase {

        public BlockContainer Blocks { get; } = new BlockContainer();

        public MessageCollection Messages { get; } = new MessageCollection();

        public ICommand ClearCommand => new RelayCommand(() => {
            Messages.Clear();
        });

        public ICommand Exit => new RelayCommand(() => {
            Application.Current.Shutdown();
        });

        public ICommand SettingsCommand => new RelayCommand(() => {
            var settingsDialog = new SettingsDialog();
            settingsDialog.ShowDialog();
        });

        public ICommand OpenCommand => new RelayCommand(async() => {

            var openFileDialog = new OpenFileDialog
            {
                DefaultExt = "dat",
                Title = "Select bitcoin block",
                Multiselect = true,
                InitialDirectory = Settings.Default.InputPath
            };

            if (openFileDialog.ShowDialog() == false)
                return;

            try{
                Mouse.OverrideCursor = Cursors.Wait;

                await new OpenFiles(Blocks, Messages).Execute(openFileDialog.FileNames);
            }
            finally{
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand ReadTransaction => new RelayCommand(() => {

            var openTransactionDialog = new OpenTransactionsDialog();

            if (openTransactionDialog.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                new ReadTransaction(Blocks, Messages).Execute(openTransactionDialog.ViewModel.Transaction);
            }
            catch (Exception ex) {
                Messages.Add($"Exception: {ex.Message}", MessageType.Error);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand WalkAddresses => new RelayCommand(() => {

            var openTransactionDialog = new OpenTransactionsDialog();

            if (openTransactionDialog.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                new WalkAddress(Blocks, Messages).Execute(openTransactionDialog.ViewModel.Transaction);
            }
            catch (Exception ex) {
                Messages.Add($"Exception: {ex.Message}", MessageType.Error);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand DownloadFile => new RelayCommand(() => {

            var openTransactionDialog = new OpenTransactionsDialog();

            if (openTransactionDialog.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                new DownloadFile(Blocks, Messages).Download(openTransactionDialog.ViewModel.Transaction.Split('\r', '\n'));
            }
            catch (Exception ex) {
                Messages.Add(ex.Message, MessageType.Error);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand DownloadSatoshiUploadedFile => new RelayCommand(() => {

            var openTransactionDialog = new OpenTransactionsDialog();

            if (openTransactionDialog.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                new DownloadSatoshi(Blocks, Messages).Download(openTransactionDialog.ViewModel.Transaction);
            }
            catch (InvalidDataException ex) {
                Messages.Add(ex.Message, MessageType.Error);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        

        public ICommand DownloadTxInputFile => new RelayCommand(() => {

            var openTransactionDialog = new OpenTransactionsDialog();

            if (openTransactionDialog.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                new DownloadTxInputFile(Blocks, Messages).Download(openTransactionDialog.ViewModel.Transaction);
            }
            catch (InvalidDataException ex) {
                Messages.Add(ex.Message, MessageType.Error);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand SearchForKnownExtensions => new RelayCommand(async() => {

            var openBlocksDialog = new OpenBlocksDialog();

            if (openBlocksDialog.ShowDialog() == false)
                return;

            try{
                Mouse.OverrideCursor = Cursors.Wait;

                await new KnownExtensions(Messages).Search(openBlocksDialog.ViewModel.Start, openBlocksDialog.ViewModel.Stop);
            }
            finally{
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand SearchForHighFees => new RelayCommand(async () => {

            var openBlocksDialog = new OpenBlocksDialog();

            if (openBlocksDialog.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                await new HighFees(Messages).Search(openBlocksDialog.ViewModel.Start, openBlocksDialog.ViewModel.Stop);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand SearchSatoshiUploads => new RelayCommand(async() => {

            var openBlocksDialog = new OpenBlocksDialog();

            if (openBlocksDialog.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                await new SatoshiUploads(Messages).Search(openBlocksDialog.ViewModel.Start, openBlocksDialog.ViewModel.Stop);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand SearchHash160Zero => new RelayCommand(async () => {

            var openBlocksDialog = new OpenBlocksDialog();

            if (openBlocksDialog.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                await new SearchHash160Zero(Messages).Search(openBlocksDialog.ViewModel.Start, openBlocksDialog.ViewModel.Stop);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand SearchForTransactionId => new RelayCommand(async() => {

            var searchTransaction = new SearchTransaction();

            if (searchTransaction.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                await new SearchTransactionId(Messages).Search(searchTransaction.ViewModel.Transaction, searchTransaction.ViewModel.Start, searchTransaction.ViewModel.Stop);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand SearchForByteArray => new RelayCommand(async () => {

            var byteArrayDialog = new ByteArrayDialog();

            if (byteArrayDialog.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                await new SearchByteArray(Messages).Search(byteArrayDialog.ViewModel.ByteText, byteArrayDialog.ViewModel.Start, byteArrayDialog.ViewModel.Stop);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand WalkUpEntireTransaction => new RelayCommand(async () => {

            var searchTransaction = new SearchTransaction();

            if (searchTransaction.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                await new WalkUpEntireTransaction(Messages).Search(searchTransaction.ViewModel.Transaction, searchTransaction.ViewModel.Start, searchTransaction.ViewModel.Stop);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand WalkDownEntireTransaction => new RelayCommand(async () => {

            var searchTransaction = new SearchTransaction();

            if (searchTransaction.ShowDialog() == false)
                return;

            try {
                Mouse.OverrideCursor = Cursors.Wait;

                await new WalkDownEntireTransaction(Messages).Search(searchTransaction.ViewModel.Transaction, searchTransaction.ViewModel.Start, searchTransaction.ViewModel.Stop);
            }
            finally {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        });

        public ICommand QueryTextMessages => new RelayCommand(() => {
            new QueryTextMessages(Blocks, Messages).Execute();
        });

        

        public ICommand WalkHashTable => new RelayCommand(async () => {

            var dialog = new OpenTransactionsDialog();

            if (dialog.ShowDialog() == false)
                return;

            await new WalkHashTable(Messages).Search(dialog.ViewModel.Transaction);
        });

        public ICommand QueryBase64 => new RelayCommand(async() => {

            var openBlocksDialog = new OpenBlocksDialog();

            if (openBlocksDialog.ShowDialog() == false)
                return;

            await new QueryBase64Messages(Messages).Search(openBlocksDialog.ViewModel.Start, openBlocksDialog.ViewModel.Stop);
        });

        public ICommand SearchAesKeys => new RelayCommand(async () => {

            var openBlocksDialog = new OpenBlocksDialog();

            if (openBlocksDialog.ShowDialog() == false)
                return;

            await new QueryAesKeys(Messages).Search(openBlocksDialog.ViewModel.Start, openBlocksDialog.ViewModel.Stop);
        });

        public ICommand SearchWikileaksHashes => new RelayCommand(async () => {

            var openBlocksDialog = new OpenBlocksDialog();

            if (openBlocksDialog.ShowDialog() == false)
                return;

            await new SearchWikileakHashes(Messages).Search(openBlocksDialog.ViewModel.Start, openBlocksDialog.ViewModel.Stop);
        });

        public ICommand QuerySeveralTxOutsFile => new RelayCommand(() => {
            new QuerySeveralTxOuts(Blocks, Messages).Execute();
        });
    }
}
