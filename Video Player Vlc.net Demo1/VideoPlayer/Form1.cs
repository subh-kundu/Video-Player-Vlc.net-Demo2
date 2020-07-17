using System;
using System.IO;
using System.Linq;
using System.Drawing;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using Vlc.DotNet.Core.Interops;
using Vlc.DotNet.Core.Interops.Signatures;
using System.Management.Instrumentation;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
namespace VideoPlayer
{
    //subh kundu mail me subh.knd@gmail.com

    public partial class Form1 : Form
    {

        
        int volume { get; set; }

        public Form1()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            this.volume = volumeTrackBar.Value;
            myVlcControl.VideoOutChanged += myVlcControl_VideoOutChanged;
        }
        
        public int a = 0;
        public int c = 0;
        public delegate void UpdateControlsDelegate();

        private bool togglePP = true;

        private void OnVlcControlNeedLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (IntPtr.Size == 4)
                e.VlcLibDirectory = new DirectoryInfo(@"..\..\..\lib\x86\");
            else
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"..\..\..\lib\x64\"));

            //var mediaOptions = new String[] { ":http-user-agent=1234" };
            //example
            //"--video-filter=croppadd{paddtop=100}"
            //"--video-filter=croppadd{cropleft=100}"

            //myVlcControl.VlcMediaplayerOptions = new string[] { "--video-filter=croppadd{paddtop=100, cropleft=100}" };

            ////myVlcControl.VlcMediaplayerOptions = new string[] { "--video-filter=transform", "--transform-type=hflip" }; // "90", "180", "270", "hflip", "vflip", "transpose", "antitranspose"
            ////myVlcControl.VlcMediaplayerOptions = new string[] {"--quiet", "--video-filter=transform", "--transform-type=180" };
            ////myVlcControl.VlcMediaplayerOptions = new string[] { "--sub-source=marq{marquee=Hello,timeout=5000,position=8}" };
            ////myVlcControl.VlcMediaplayerOptions = new string[] { "--file-logging", "-vvv", "--logfile=Logs.log" };
            ////myVlcControl.VlcMediaplayerOptions = new string[] {  ":sout=#file{dst="+Path.Combine(Environment.CurrentDirectory, "output.mov")+"}", ":sout-keep"};
            ////myVlcControl.VlcMediaplayerOptions = new string[] { "--no-video-title", "--video-title-show", "--video-title-timeout=4000", "--osd" };

            ////myVlcControl.VlcMediaplayerOptions = new string[] { "--video-filter=magnify" };
            //if (Disable_Vis.Checked)
            //{

            //}
            //if (Scope_Vis.Checked)
            //{
            //    myVlcControl.VlcMediaplayerOptions = new string[] { "--audio-visual=visual", "--effect-list=scope" };
            //}
            //if (Spectrum_Vis.Checked)
            //{
            //    myVlcControl.VlcMediaplayerOptions = new string[] { "--audio-visual=visual", "--effect-list=spectrum" };
            //}
            //if (Spectrometer_Vis.Checked)
            //{
            //    myVlcControl.VlcMediaplayerOptions = new string[] { "--audio-visual=visual", "--effect-list=spectrometer" };
            //}


            //this.myVlcControl.VlcMediaplayerOptions = new string[] { "--deinterlace=on", "--video-filter=deinterlace", "--deinterlace-mode=linear" };


            //myVlcControl.EndInit();

            //if (myVlcControl.VlcMediaplayerOptions == null)
            //{
            //    myVlcMediaPlayer = new VlcMediaPlayer(myVlcControl.VlcLibDirectory);
            //}
            //else
            //{
            //    myVlcMediaPlayer = new VlcMediaPlayer(myVlcControl.VlcLibDirectory, myVlcControl.VlcMediaplayerOptions);
            //}
            ////myVlcControl.VlcMediaplayerOptions = new string[] { "--no-ts-pcr-offsetfix" };

            //FOR STREAMING...
            //myVlcControl.VlcMediaplayerOptions = new string[] { "--network-caching=150" + "--sout-mux-caching=150" };


            //myVlcControl.VlcMediaplayerOptions = new string[] { "--file-caching=5500" };
            //myVlcControl.VlcMediaplayerOptions = new string[] { "--network-caching=300" };


            //myVlcControl.VlcMediaplayerOptions = new string[] { ":network-caching=300" };
            //myVlcControl.VlcMediaplayerOptions = new string[] { ":sout=#duplicate{dst=display,dst=std {access=udp,mux=ts,dst=224.100.0.1:1234}}" };
            //myVlcControl.VlcMediaplayerOptions = new string[] { ":sout=#transcode{vcodec=h264,vb=800,acodec=mpga,ab=128,channels=2,samplerate=44100,scodec=none}:udp{mux=ts,dst=224.100.0.1:1234}" };
            //myVlcControl.VlcMediaplayerOptions = new string[] { ":sout=#duplicate{dst=display,dst=std {access=udp,mux=ts,dst=224.100.0.1:1234}}" };
            //myVlcControl.VlcMediaplayerOptions = new string[] { ":sout=#duplicate{dst=udp{dst=224.100.0.1:1234},dst=display}" };


            //myVlcControl.VlcMediaplayerOptions = new string[] { ":sout=#duplicate{dst=rtp{sdp=rtsp://127.0.0.1:1000/},dst=display}",
            //    ":sout-all",
            //    ":sout-keep" };



            myVlcControl.VlcMediaplayerOptions = new string[] { "dshow:// :dshow-vdev=screen-capture-recorder", ":dshow-adev=virtual-audio-capturer" };



            if (!e.VlcLibDirectory.Exists)
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.Description = "Select Vlc libraries folder.";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void DirectPlaybutton_Click(object sender, EventArgs e)
        {
            myVlcControl.SetMedia(new Uri(textBox1.Text), myVlcControl.VlcMediaplayerOptions);
            //myVlcControl.Play();

            //myVlcControl.SetMedia("http://mywebsite.net:8000/live/me1/me2/135.ts", myVlcControl.VlcMediaplayerOptions);
            //myVlcControl.Play();

            //myVlcControl.VlcMediaPlayer.SetUserAgent("");

            //myVlcControl.SetMedia(new Uri(@"D:\AshantiTV\User\Videos\Fillers\vlc-record.mp4"), myVlcControl.VlcMediaplayerOptions);
            myVlcControl.Play();

        }


private void button2_Click(object sender, EventArgs e)
        {
            myVlcControl.Stop();
            timer1.Enabled = false;
        }

        private void muteButton_Click(object sender, EventArgs e)
        {
            myVlcControl.Audio.ToggleMute();
        }

        private void myVlcControl_VideoOutChanged(object sender, VlcMediaPlayerVideoOutChangedEventArgs e)
        {
            myVlcControl.Audio.Volume = volume;
        }
        private void volumeTrackBar_Scroll(object sender, EventArgs e)
        {
            this.volume = volumeTrackBar.Value;
            myVlcControl.Audio.Volume = volume;
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

                TimeSpan time = TimeSpan.FromSeconds((int)myVlcControl.VlcMediaPlayer.Length / 1000);
                trackBar2.Maximum = (int)myVlcControl.VlcMediaPlayer.Length / 1000;
                string str = time.ToString(@"hh\:mm\:ss");
                label2.Text = str;

                TimeSpan seek = TimeSpan.FromSeconds((int)myVlcControl.VlcMediaPlayer.Time / 1000);
                trackBar2.Value = (int)myVlcControl.VlcMediaPlayer.Time / 1000;
                string strr = seek.ToString(@"hh\:mm\:ss");
                label1.Text = strr;
                timer2.Start();
            }
            catch (Exception)
            {
                timer1.Enabled = false;
            }
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            myVlcControl.VlcMediaPlayer.Time = trackBar2.Value * 1000;
            TimeSpan seeking = TimeSpan.FromSeconds((int)myVlcControl.VlcMediaPlayer.Time / 1000);
            string strr = seeking.ToString(@"hh\:mm\:ss");
            label1.Text = strr;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            myVlcControl.TakeSnapshot(new System.IO.FileInfo(@"D:\screenshot_\" + System.IO.Path.GetFileNameWithoutExtension("fileName") + ".jpg"), 320, 240);
        }


        private void hizYazdir()
        {
            label5.Text = "Mevcut Hız:" + Math.Round(myVlcControl.Rate, 2).ToString() + "x";
        }


        private void button5_Click(object sender, EventArgs e)
        {
            myVlcControl.Rate += (float)0.10;
            hizYazdir();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            myVlcControl.Video.AspectRatio = comboBox1.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            myVlcControl.Rate = 1.0f;
            hizYazdir();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            myVlcControl.Rate -= (float)0.10;
            hizYazdir();
        }

        private void button9_Click(object sender, EventArgs e)
        {

            myVlcControl.Time += 1000;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            myVlcControl.Time -= 1000;
        }

        private void button10_Click(object sender, EventArgs e)
        {

            string item = System.Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\";

            string savePath = string.Format("{0}{1:yyyy_MM_dd_HH-mm-ss}.mp4", item, System.DateTime.Now);

            var mediaOptions = new string[]
            {
            //    ":sout=#file{dst="+Path.Combine(Environment.CurrentDirectory, "output.mp4")+"}",
            //    ":brightness="+50,
            //    ":sout-keep"
            //};

            //":sout=#transcode{vcodec=theo,vb=800,scale=1,acodec=flac,ab=128,channels=2,samplerate=44100}:std{access=file,mux=ogg,dst=d:\\123.mp4}"
            //};


            //":sout=#duplicate{dst=std{access=file,mux=mp4,dst='" + savePath + ".mp4'},dst=display}"
            //};
                       


            ":sout=#duplicate{dst=std{access=file,mux=" + "mp4" + ",dst='" + savePath + "'},dst=display}" };

            myVlcControl.SetMedia(new FileInfo(textBox1.Text), mediaOptions);
            myVlcControl.Play();


            //----------------------------

            //myVlcControl.AddTarget("fake://", new string[] {":no-overlay", ":input-repeat=-1", 
            //        ":vout-filter=adjust", ":fake-file=" + fileName.Trim(), ":fake-fps=1",
            //        ":brightness="+50, ":fake-caching=100"}, ref playListId);

            //myVlcControl.SetMedia(new FileInfo(textBox1.Text), mediaOptions);


            //string path = ....
            //LocationMedia media = new LocationMedia(path);        (":sout=#transcode{vcodec=theo,vb=800,scale=1,acodec=flac,ab=128,channels=2,samplerate=44100}:std{access=file,mux=ogg,dst=C:\\Users\\hsilva\\Desktop\\123.mp4}");
            //myVlcControl.SetMedia = media;
            //myVlcControl.SetMedia(new FileInfo(textBox1.Text), ":sout=#transcode{vcodec=theo,vb=800,scale=1,acodec=flac,ab=128,channels=2,samplerate=44100}:std{access=file,mux=ogg,dst=d:\\123.mp4}");
            //myVlcControl.Play();


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            myVlcControl.Video.CropGeometry = comboBox2.Text;
        }
        private void Openbutton_Click(object sender, EventArgs e)
        {
            string vlcPath = null;
            audioTracksToolStripMenuItem.DropDownItems.Clear();
            videoTracksToolStripMenuItem.DropDownItems.Clear();
            subTrackToolStripMenuItem.DropDownItems.Clear();
            OpenFileDialog MyDialog = new OpenFileDialog();
            MyDialog.RestoreDirectory = true;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = MyDialog.FileName;
                vlcPath = null + MyDialog.FileName + null;                
                PlayMedia(vlcPath);
                myVlcControl.VlcMediaPlayer.SetVideoTitleDisplay(Position.Bottom, 5000);
                vlcControl1.VlcMediaPlayer.SetVideoTitleDisplay(Position.Bottom, 5000);
            }
        }

        public void PlayMedia(string vlcPath)
        {
            this.myVlcControl.SetMedia(new Uri(vlcPath), myVlcControl.VlcMediaplayerOptions);
            this.Text = myVlcControl.GetCurrentMedia().Title + " myVideoLanClient Home implemention";
            this.myVlcControl.Play();
            timer1.Start();

            //vlcControl1.SetMedia(new Uri("udp://@224.100.0.1:1234"));
            //vlcControl1.Play();
            //Thread.Sleep(1000);



        }


        public void GetAudioTrack()
        {
            foreach (var audioTracks in myVlcControl.Audio.Tracks.All)
            {
                ToolStripMenuItem tMenu = new ToolStripMenuItem();
                tMenu = audioTracksToolStripMenuItem;
                tMenu.DropDownItems.Add(audioTracks.Name);
                tMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(AudioTrackHandleDynamicMenuClick);
            }
        }
        private void AudioTrackHandleDynamicMenuClick(object sender, ToolStripItemClickedEventArgs e)
        {
            var parent = (ToolStripMenuItem)sender;
            int index = parent.DropDownItems.IndexOf(e.ClickedItem);
            myVlcControl.Audio.Tracks.Current = myVlcControl.Audio.Tracks.All.ElementAt(index);
            foreach (ToolStripMenuItem item in audioTracksToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
                ((ToolStripMenuItem)audioTracksToolStripMenuItem.DropDownItems[index]).Checked = true;
            }
        }
        public void GetVideoTrack()
        {
            foreach (var videoTracks in myVlcControl.Video.Tracks.All.ToArray())
            {
                ToolStripMenuItem tMenu = new ToolStripMenuItem();
                tMenu = videoTracksToolStripMenuItem;
                tMenu.DropDownItems.Add(videoTracks.Name);                
                tMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(VideoTrackHandleDynamicMenuClick);            
            }
        }
        private void VideoTrackHandleDynamicMenuClick(object sender, ToolStripItemClickedEventArgs e)
        {
            var parent = (ToolStripMenuItem)sender;
            int index = parent.DropDownItems.IndexOf(e.ClickedItem);            
            myVlcControl.Video.Tracks.Current = myVlcControl.Video.Tracks.All.ElementAt(index);
            foreach (ToolStripMenuItem item in videoTracksToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
                ((ToolStripMenuItem)videoTracksToolStripMenuItem.DropDownItems[index]).Checked = true;
            }
        }
        public void GetSubtitle()
        {
            foreach (var subtitleTracks in myVlcControl.SubTitles.All.ToArray())
            {
                ToolStripMenuItem tMenu = new ToolStripMenuItem();
                tMenu = subTrackToolStripMenuItem;
                tMenu.DropDownItems.Add(subtitleTracks.Name);
                tMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(SubtitleHandleDynamicMenuClick);
            }
        }
        private void SubtitleHandleDynamicMenuClick(object sender, ToolStripItemClickedEventArgs e)
        {
            var parent = (ToolStripMenuItem)sender;
            int index = parent.DropDownItems.IndexOf(e.ClickedItem);
            myVlcControl.SubTitles.Current = myVlcControl.SubTitles.All.ElementAt(index);
            foreach (ToolStripMenuItem item in subTrackToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
                ((ToolStripMenuItem)subTrackToolStripMenuItem.DropDownItems[index]).Checked = true;
            }
        }

        public void GetTitle()
        {
            //foreach (var TitleTracks in 
            //{
            //    ToolStripMenuItem tMenu = new ToolStripMenuItem();
            //    tMenu = subTrackToolStripMenuItem;
            //    tMenu.DropDownItems.Add(subtitleTracks.Name);
            //    tMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(SubtitleHandleDynamicMenuClick);
            //}
        }

        public void GetChapter()
        {
            //foreach (var videoTracks in myVlcControl.Chapter.Count)
            //{
            //    ToolStripMenuItem tMenu = new ToolStripMenuItem();
            //    tMenu = videoTracksToolStripMenuItem;
            //    tMenu.DropDownItems.Add(videoTracks.Name);
            //    //((ToolStripMenuItem)tMenu.DropDownItems[videoTracks.ID]).Checked = true;
            //    tMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(VideoTrackHandleDynamicMenuClick);
            //}

            //foreach (var playchapter in myVlcControl.Chapter.Count.ToString())
            //{
            //    ToolStripMenuItem tMenu = new ToolStripMenuItem();
            //    tMenu = subTrackToolStripMenuItem;
            //    tMenu.DropDownItems.Add(playchapter.Name);
            //    //((ToolStripMenuItem)tMenu.DropDownItems[videoTracks.ID]).Checked = true;
            //    tMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(SubtitleHandleDynamicMenuClick);

            //}



            ToolStripMenuItem tMenu = new ToolStripMenuItem();

            tMenu = chapterToolStripMenuItem;
            tMenu.DropDownItems.Clear();
            for (int i = 0; i < myVlcControl.Chapter.Count; i++)
            {
                //tMenu.DropDownItems.Add("Chapter " + i.ToString());
                //tMenu.DropDownItems.Add();

            }

            tMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(HandleDynamicMenuClick);

            

        }

        public void AudioSample()
        {
            if (myVlcControl.Audio != null)
            {
                var outputs = myVlcControl.Audio.Outputs;
                if (outputs != null)
                {
                    myCbxAudioOutputs.DataSource = new List<AudioOutputDescription>(outputs.All);
                    myCbxAudioOutputs.DisplayMember = "Description";
                    myCbxAudioOutputs.Enabled = true;
                    myCbxAudioOutputs.SelectedIndex = 0;
                    myCbxAudioOutputs.SelectedValueChanged += (o, a) =>
                    {
                        var val = myCbxAudioOutputs.SelectedValue;
                        if (val != null)
                        {
                            var output = val as AudioOutputDescription;
                            if (output != null)
                            {
                                outputs.Current = output;                                
                            }
                        }
                    };

                }
            }

            //foreach (AudioOutputDevice device in AudioOutputDeviceTypes.Mono)
            //{

            //}

        }


        



        public void Method1()
        {
            ToolStripMenuItem tMenu = new ToolStripMenuItem();

            tMenu = chapterToolStripMenuItem;
            tMenu.DropDownItems.Clear();
            for (int i = 0; i < myVlcControl.Chapter.Count; i++)
            {
                tMenu.DropDownItems.Add("Chapter " + i.ToString());

            }

            tMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(HandleDynamicMenuClick);
        }

        void HandleDynamicMenuClick(object sender, ToolStripItemClickedEventArgs e)
        {
            string msg = String.Format("Item clicked: {0}", e.ClickedItem.Text);
            MessageBox.Show(msg);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            myVlcControl.Chapter.Next();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            myVlcControl.Chapter.Previous();            
        }

        private void OnVlcPlaying(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            Id3GetTags();
        }
         
               



        #region Image Adjustment
        private void HuetrackBar_Scroll(object sender, EventArgs e)
        {
            myVlcControl.Video.Adjustments.Hue = HuetrackBar.Value;
            Huelabel.Text = HuetrackBar.Value.ToString();
        }
        private void BrightnesstrackBar_Scroll(object sender, EventArgs e)
        {
            float value = BrightnesstrackBar.Value / 10.0f;
            BrightnesstrackBar.Text = value.ToString("0.0");
            myVlcControl.Video.Adjustments.Brightness = value;
            Brightnesslabel.Text = value.ToString();
        }
        private void ContrasttrackBar_Scroll(object sender, EventArgs e)
        {
            float value = ContrasttrackBar.Value / 10.0f;
            ContrasttrackBar.Text = value.ToString("0.0");
            this.myVlcControl.Video.Adjustments.Contrast = value;
            Contrastlabel.Text = value.ToString();
        }
        private void SaturationtrackBar_Scroll(object sender, EventArgs e)
        {
            float value = SaturationtrackBar.Value / 10.0f;
            SaturationtrackBar.Text = value.ToString("0.0");
            this.myVlcControl.Video.Adjustments.Saturation = value;
            Saturationlabel.Text = value.ToString();
        }
        private void GammatrackBar_Scroll(object sender, EventArgs e)
        {
            float value = GammatrackBar.Value / 10.0f;
            GammatrackBar.Text = value.ToString("0.0");
            this.myVlcControl.Video.Adjustments.Gamma = value;
            Gammalabel.Text = value.ToString();
        }
        private void Resetbutton_Click(object sender, EventArgs e)
        {
            this.myVlcControl.Video.Adjustments.Hue = 0;
            this.myVlcControl.Video.Adjustments.Brightness = 1.0f;
            this.myVlcControl.Video.Adjustments.Contrast = 1.0f;
            this.myVlcControl.Video.Adjustments.Saturation = 1.0f;
            this.myVlcControl.Video.Adjustments.Gamma = 1.0f;
        }
        #endregion

        #region Add Logo
        private void LogoOpenbutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog MyDialog = new OpenFileDialog();
            MyDialog.RestoreDirectory = true;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                LogotextBox.Text = MyDialog.FileName;
            }
        }
        private void AddLogocheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Vlc.DotNet.Core.ILogoManagement logo = myVlcControl.Video.Logo;
            if (AddLogocheckBox.Checked == true)
            {
                groupBox2.Enabled = true;
                LogoSettings();

            }
            else
            {
                groupBox2.Enabled = false;
                logo.Enabled = false;
            }
        }
        private void LogoSettings()
        {
            Vlc.DotNet.Core.ILogoManagement logo = myVlcControl.Video.Logo;
            string fileName = LogotextBox.Text;
            logo.File = fileName;
            logo.Enabled = true;
            if (TextPositioncomboBox.Text == "Center")
            {
                logo.Position = 0; //> { 0, 1, 2, 4, 8, 5, 6, 9, 10 } : Logo po
            }            
            if (TextPositioncomboBox.Text == "Left")
            {
                logo.Position = 1;
            }
            if (TextPositioncomboBox.Text == "Right")
            {
                logo.Position = 2;
            }
            if (TextPositioncomboBox.Text == "Top")
            {
                logo.Position = 4;
            }
            if (TextPositioncomboBox.Text == "Bottom")
            {
                logo.Position = 8;
            }
            if (TextPositioncomboBox.Text == "Top-Left")
            {
                logo.Position = 5;
            }
            if (TextPositioncomboBox.Text == "Top-Right")
            {
                logo.Position = 6;
            }
            if (TextPositioncomboBox.Text == "Bottom-Left")
            {
                logo.Position = 9;
            }
            if (TextPositioncomboBox.Text == "Bottom-Right")
            {
                logo.Position = 10;
            }
            logo.Position = Convert.ToInt16(LogoPositioncomboBox.SelectedIndex);
            logo.X = Convert.ToInt16(LogoLeftUpDown.Value);
            logo.Y = Convert.ToInt16(LogoTopUpDown.Value);
            logo.Opacity = LogotrackBar.Value;
        }
        private void LogotrackBar_Scroll(object sender, EventArgs e)
        {
            LogoSettings();
        }
        private void LogoTopUpDown_ValueChanged(object sender, EventArgs e)
        {
            LogoSettings();
        }
        private void LogoLeftUpDown_ValueChanged(object sender, EventArgs e)
        {
            LogoSettings();
        }
        private void LogoPositionUpDown_ValueChanged(object sender, EventArgs e)
        {
            LogoSettings();
        }
        private void LogotextBox_TextChanged(object sender, EventArgs e)
        {
            LogoSettings();
        }
        private void LogoPositioncomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogoSettings();
        }
        #endregion

        #region Add Text
        private void TextSettings()
        {
            Vlc.DotNet.Core.IMarqueeManagement myVLCText = myVlcControl.Video.Marquee;
            myVLCText.Enabled = true;

            if (TextPositioncomboBox.Text == "Center")
            {
                myVLCText.Position = 0;
            }
            if (TextPositioncomboBox.Text == "Left")
            {
                myVLCText.Position = 1;
            }
            if (TextPositioncomboBox.Text == "Right")
            {
                myVLCText.Position = 2;
            }
            if (TextPositioncomboBox.Text == "Top")
            {
                myVLCText.Position = 4;
            }
            if (TextPositioncomboBox.Text == "Bottom")
            {
                myVLCText.Position = 8;
            }
            if (TextPositioncomboBox.Text == "Top-Left")
            {
                myVLCText.Position = 5;
            }
            if (TextPositioncomboBox.Text == "Top-Right")
            {
                myVLCText.Position = 6;
            }
            if (TextPositioncomboBox.Text == "Bottom-Left")
            {
                myVLCText.Position = 9;
            }
            if (TextPositioncomboBox.Text == "Bottom-Right")
            {
                myVLCText.Position = 10;
            }
            myVLCText.Size = 52;
            myVLCText.Text = myVLCtextBox.Text;
            myVLCText.X = 0;
            myVLCText.Y = 0;
            myVLCText.Opacity = 255;
            myVLCText.Color = Color.White.ToArgb();
        }
        private void AddTextcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Vlc.DotNet.Core.IMarqueeManagement myVLCText = myVlcControl.Video.Marquee;
            if (AddTextcheckBox.Checked == true)
            {
                groupBox3.Enabled = true;
                TextSettings();
            }
            else
            {
                groupBox2.Enabled = false;
                myVLCText.Enabled = false;
            }
        }
        private void TextPositioncomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextSettings();
        }
        #endregion

        private void ImageAdjustcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ImageAdjustcheckBox.Checked == true)
            {
                groupBox1.Enabled = true;
                myVlcControl.Video.Adjustments.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
                myVlcControl.Video.Adjustments.Enabled = false;
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("https://download.videolan.org/pub/videolan/vlc/3.0.3/", "myVLC", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void button11_Click(object sender, EventArgs e)
        {

            VlcMedia vlcm = myVlcControl.GetCurrentMedia();
            foreach (Vlc.DotNet.Core.Interops.MediaTrack i in vlcm.Tracks)
            {
                listBox1.Items.Add("--------------------------------------------------------------------------------------------------------");
                listBox1.Items.Add("Stream: " + i.Id);
                listBox1.Items.Add("--------------------------------------------------------------------------------------------------------");                
                listBox1.Items.Add("Language: " + i.Language);
                listBox1.Items.Add("Description: " + i.Description);
                listBox1.Items.Add("Type: " + i.Type);
                listBox1.Items.Add("Bitrate: " + i.Bitrate / 1000 + " kb/s");

                listBox1.Items.Add(myVlcControl.VlcMediaPlayer.FramesPerSecond.ToString());

            }
        }


        private void addSubtitleFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog MyDialog = new OpenFileDialog();
            MyDialog.RestoreDirectory = true;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                SubtitletextBox.Text = MyDialog.FileName;
            }
        }

        private void Pausebutton_Click(object sender, EventArgs e)
        {
            if (togglePP == true)
            {
                Pausebutton.Text = "Play";
                myVlcControl.Pause();
                togglePP = false;
            }

            else if (togglePP == false)
            {
                myVlcControl.Pause();
                Pausebutton.Text = "Pause";
                togglePP = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            audioTracksToolStripMenuItem.DropDownItems.Clear();
            videoTracksToolStripMenuItem.DropDownItems.Clear();
            subTrackToolStripMenuItem.DropDownItems.Clear();
            this.GetAudioTrack();
            this.GetVideoTrack();
            this.GetSubtitle();

        }

        private void DefaultVisual(object sender, EventArgs e)
        {
            if (sender == Disable_Vis)
            {

                Disable_Vis.Checked = true;
                Scope_Vis.Checked = false;
                Spectrum_Vis.Checked = false;
                Spectrometer_Vis.Checked = false;
            }
            else if (sender == Scope_Vis)
            {
                
                Disable_Vis.Checked = false;
                Scope_Vis.Checked = true;
                Spectrum_Vis.Checked = false;
                Spectrometer_Vis.Checked = false;
            }
            else if (sender == Spectrum_Vis)
            {
                //string spectrum_vis = "--effect - list = spectrum";
                Disable_Vis.Checked = false;
                Scope_Vis.Checked = false;
                Spectrum_Vis.Checked = true;
                Spectrometer_Vis.Checked = false;
            }
            else if (sender == Spectrometer_Vis)
            {

                Disable_Vis.Checked = false;
                Scope_Vis.Checked = false;
                Spectrum_Vis.Checked = false;
                Spectrometer_Vis.Checked = true;
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            AudioSample();
            this.vlcControl1.Spu = -1;
        }
       

        private void Id3GetTags()
        {
            try
            {
                var mediaInformations = myVlcControl.GetCurrentMedia();
                TitleTextBox.Text = mediaInformations.Title;
                ArtistTextBox.Text = mediaInformations.Artist;
                AlbumTextBox.Text = mediaInformations.Album;
                DateTextBox.Text = mediaInformations.Date;
                GenreTextBox.Text = mediaInformations.Genre;
                TrackNumberTextBox.Text = mediaInformations.TrackNumber;
                NowPlayingTextBox.Text = mediaInformations.NowPlaying;
                LanguageTextBox.Text = mediaInformations.Language;
                PublisherTextBox.Text = mediaInformations.Publisher;
                CopyrightTextBox.Text = mediaInformations.Copyright;
                EncodedbyTextBox.Text = mediaInformations.EncodedBy;
                CommentsTextBox.Text = mediaInformations.Description;
                //AlbumArtpictureBox.Image = Image.FromFile(mediaInformations);
                
            }
            catch (Exception)
            {
                                
            }
            
        }

        #region AudioChannel
        private void monoToolStripMenu_Click(object sender, EventArgs e)
        {
            myVlcControl.VlcMediaPlayer.Audio.Channel = 0;
        }
        private void stereoToolStripMenu_Click(object sender, EventArgs e)
        {
            myVlcControl.VlcMediaPlayer.Audio.Channel = 1;
        }
        private void leftToolStripMenu_Click(object sender, EventArgs e)
        {
            myVlcControl.VlcMediaPlayer.Audio.Channel = 3;
        }
        private void rightToolStripMenu_Click(object sender, EventArgs e)
        {
            myVlcControl.VlcMediaPlayer.Audio.Channel = 4;
        }
        private void reverseStereoToolStripMenu_Click(object sender, EventArgs e)
        {
            myVlcControl.VlcMediaPlayer.Audio.Channel = 2;
        }
        private void headphonesToolStripMenu_Click(object sender, EventArgs e)
        {
            myVlcControl.VlcMediaPlayer.Audio.Channel = 8;
        }
        private void originalToolStripMenu_Click(object sender, EventArgs e)
        {
            myVlcControl.VlcMediaPlayer.Audio.Channel = 0;
        }
        #endregion





        private void timer2_Tick(object sender, EventArgs e)
        {
            if (myVlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Ended)
            {
                PlayMedia(textBox1.Text);
            }
        }
           

        private void button14_Click(object sender, EventArgs e)
        {
            //224.100.0.1:1234

            //string[] options = { ":sout=#duplicate{dst=display,dst=std{access=file,mux=asf,dst=\"F:\\My-Output-Video-Filename.asf\"}}" };
            vlcControl1.SetMedia(new Uri("udp://@224.100.0.1:1234"));
            //vlcControl1.SetMedia(new Uri("rtsp://127.0.0.1:1000"));
            vlcControl1.Play();

            
        }

        private void vlcControl1_VlcLibDirectoryNeeded(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (IntPtr.Size == 4)
                e.VlcLibDirectory = new DirectoryInfo(@"..\..\..\lib\x86\");
            else
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"..\..\..\lib\x64\"));
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            vlcControl1.Audio.Volume = trackBar3.Value;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.myVlcControl.SetMedia("dshow://", myVlcControl.VlcMediaplayerOptions);            
            this.myVlcControl.Play();
        }
    }
}