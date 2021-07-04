using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // tạo form
using System.Speech;
using System.Speech.Recognition; // namespace nhận diện giọng nói
using System.Speech.Synthesis; // namepsace để tạo giọng nói ...
using System.IO;// namespace chứa các thao tác làm việc với file ( như đọc , ghi , xóa file)

namespace Virtual_Assistant
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();// khởi tạo công cụ nhận diện giọng nói
        SpeechSynthesizer speech = new SpeechSynthesizer(); // khởi tạo tạo giọng nói 
        System.Media.SoundPlayer music = new System.Media.SoundPlayer(); // thao tác với file âm thanh 

        public Form1()
        {
            InitializeComponent();

            // Tạo một đối tượng Choices có chứa một tập hợp các cụm từ thay thế , được lưu trong file grammar.txt
            Choices choices = new Choices();
            string[] text = File.ReadAllLines(Environment.CurrentDirectory + "//grammar.txt");
            choices.Add(text);

            Grammar grammar = new Grammar(new GrammarBuilder(choices));
            recEngine.LoadGrammar(grammar);
            recEngine.SetInputToDefaultAudioDevice();
            recEngine.RecognizeAsync(RecognizeMode.Multiple);// Thực hiện một hoặc nhiều hoạt động nhận dạng giọng nói không đồng bộ.
            recEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recEngine_SpeechRecognized);// Add a handler for the speech recognized event.
            recEngine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(audioLevel); // Add a handler for the AudioLevelUpdated event.
            speech.SelectVoiceByHints(VoiceGender.Female); 
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            speech.SpeakAsync("Hi , I'm Mono, how can I help you ?");
        }

        // hàm xử lý sự kiện AudioLevelUpdated 
        private void audioLevel(object sender, AudioLevelUpdatedEventArgs e)
         {

                    this.progressBar1.Maximum = 100;
                    this.progressBar1.Value = e.AudioLevel;
         }

        // hàm thực hiện các câu lệnh điều khiển
        private void Controll(string result)
        {
            richTextBox1.AppendText("You : " + result + Environment.NewLine);// viết theo từng dòng
            if (result == "What is your name")
            {

                result = "My name is Mono, how can I help you ?";
            }
            if (result == "Can I see your face")
            {
                pictureBox3.Image = Image.FromFile(@"..\..\..\Pictures\myface.jpg");
                result = "Do you think I'm pretty?";
            }
            if ( result == "No")
            {
                result = "Oh , Thank you . What can I help you ?";
            }
            if(result == "How is the weather today")
            {
                System.Diagnostics.Process.Start("https://weather.com/en-TT/weather/today/l/e09d58707a823303a77d65888f867fbe34d5d80ab1e7983a17461491a84474eb");
                result = "Show weather on the " + DateTime.Now.Day.ToString() + " in Hanoi , VietNam";

            }
            if (result == "Yes")
            {
                result = "Oh , Thank you very much , You are pretty too";
            }
            if (result == "What time is it")
            {
                result = "It is currently " + DateTime.Now.ToLongTimeString();
            }
            if (result == "Open Skype")
            {
                System.Diagnostics.Process.Start("skype://");
            }
            if (result == "Close Skype")
            {
                /*
                 Get all instances of Skype running on the local computer.
                 This will return an empty array if Skype isn't running.
                 */
                System.Diagnostics.Process[] close = System.Diagnostics.Process.GetProcessesByName("skype");
                foreach (System.Diagnostics.Process p in close)
                    p.Kill();// stop process
                result = "Closing Skype";
            }
            if (result == "Open Spotify Music")
            {
                System.Diagnostics.Process.Start("spotify://");
            }

            if (result == "Close Spotify")
            {
                System.Diagnostics.Process[] close = System.Diagnostics.Process.GetProcessesByName("spotify");
                foreach (System.Diagnostics.Process p in close)
                    p.Kill();
                result = "Closing Spotify";
            }
            if (result == "Open Google")
            {
                System.Diagnostics.Process.Start("https://www.google.com.vn/");
                result = "Opening Google";
            }

            if (result == "Open Youtube")
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/");
                result = "Opening Youtube";
            }
            if (result == "Close Google Chrome")
            {
                System.Diagnostics.Process[] close = System.Diagnostics.Process.GetProcessesByName("chrome");
                foreach (System.Diagnostics.Process p in close)
                    p.Kill();
                result = "Closing Google Chrome";
            }
            if (result == "Shut down")
            {
                Application.Exit();
            }
            if (result == "Faded")
            {
                music.SoundLocation = "Faded.wav";
                music.Play();
                result = "Playing...";
            }
            if (result == "Stop")
            {
                speech.SpeakAsyncCancelAll();
                music.Stop();
                result = "Ok , stopped";
            }
            speech.SpeakAsync(result);
            richTextBox1.AppendText("Mono : " + result + Environment.NewLine);

            // Save speech vào file
            string date = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
            string filename = "log\\" + date + ".txt";
            StreamWriter write = File.AppendText(filename);
            if (File.Exists(filename))
            {
                write.WriteLine(result);
            }
            else // nếu ko tồn tại file , tự tạo file
            {
                write.WriteLine(result);
            }
            write.Close();
        }

        // hàm xử lý nhận diện giọng nói
        private void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            string result = e.Result.Text;
            Controll(result);
        }

        // hàm xử lý giá trị trong comboBox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string result = comboBox1.SelectedItem.ToString();

            Controll(result);

        }



        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
