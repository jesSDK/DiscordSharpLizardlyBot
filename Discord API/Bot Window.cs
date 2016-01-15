using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordSharp;
using System.Threading;
using RedditSharp;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using TwitchCSharp;
using TwitchCSharp.Clients;
using TwitchCSharp.Enums;
using TwitchCSharp.Helpers;
using TwitchCSharp.Models;
using SpeedrunComSharp;


namespace Discord_API
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Disable cross threading checks 'cause im lazy
            Form.CheckForIllegalCrossThreadCalls = false;
        }


        //Declaring objects and variables//
        DiscordClient client = new DiscordClient();
        DiscordChannel dc = new DiscordChannel();
        DiscordServer ds = new DiscordServer();
        Random rnd = new Random();
        Stopwatch sw = Stopwatch.StartNew();
        static System.Timers.Timer tmr;
        static System.Timers.Timer tmr1;
        static System.Timers.Timer tmr2;
        static System.Timers.Timer tmr3;
        ArrayList list = new ArrayList();
        DiscordRole admin = new DiscordRole();
        TwitchNamedClient Twient = new TwitchNamedClient("", "", ""); //twitch auth keys here
        string clientid;
        SpeedrunComClient SRC = new SpeedrunComClient();



        //Begin actual code//

        public void commands()
        {
           
            monoFlat_TextBox2.Text += "Thread started \r\n";

            //---------------//
            //    Commands   //
            //---------------//
            client.MessageReceived += (sender2, b) =>
            {
                monoFlat_TextBox2.Text += "Event triggered\r\n";

                if (b.message.content.StartsWith("!Status"))
                {
                    client.SendMessageToChannel("Bot is alive!", dc);

                } else if (b.message.content.StartsWith("!Update")) {

                    string game;

                    game = b.message.content.Substring(8);
                    client.UpdateCurrentGame(game);
                    client.SendMessageToChannel("Game updated to: " + game, dc);


                } else if (b.message.content.StartsWith("!Uptime")) {

                    client.SendMessageToChannel("I have been alive for: " + sw.Elapsed.ToString(), dc);


                } else if (b.message.content.StartsWith("!lmgtfy")) {

                    if (b.message.author.user.username.ToString() == "kanichiwah")
                    {
                        client.SendMessageToChannel("memes r gud 2k15", dc);
                    }
                    string link;
                    link = b.message.content.Substring(8);
                    client.SendMessageToChannel("http://www.lmgtfy.com/?q=" + link, dc);


                } else if (b.message.content.StartsWith("!MemesActivate")) {

                    Random rnd = new Random();
                    int r = rnd.Next(list.Count);
                    client.SendMessageToChannel((string)list[r], dc);


                } else if (b.message.content.StartsWith("!Live"))
                {

                    foreach (Stream channel1 in Twient.GetFollowedStreams().List)
                    {
                        string strm;
                        string title;
                        strm = channel1.Channel.DisplayName.ToString();
                        title = channel1.Channel.Status.ToString();
                        client.SendMessageToChannel("Currently Live: " + strm + ": " + title + "\r\n", dc);
                    }
                } else if (b.message.content.StartsWith("!Wowie"))
                {
                    foreach (var role in b.message.author.roles)
                    {
                        if (role.id == admin.id)
                        {
                            client.SendMessageToChannel("WOWIE REALLY?", dc);
                        }
                        else
                        {
                            client.EditMessage(b.message.id, "IM AN EGGLESS LEG", dc);
                        }
                    }

                } else if (b.message.content.StartsWith("!Topic"))
                {
                    foreach (var role in b.message.author.roles)
                    {
                        if (role.id == admin.id)
                        {
                            string topic = b.message.content.Substring(7);
                            client.ChangeChannelTopic(topic, dc);
                            client.SendMessageToChannel("Channel topic set to " + topic, dc);
                        }
                        else
                        {
                            client.SendMessageToChannel("Not enough permissions ya nerd CoolCat", dc);
                        }
                    }



                } else if (b.message.content.StartsWith("!RPS"))
                {

                } else if (b.message.content.StartsWith("!AddMeme"))
                {
                    string meme;
                    meme = b.message.content.Substring(9);
                    list.Add(meme);
                    list = Properties.Settings.Default.MySetting;
                    Properties.Settings.Default.MySetting = list;
                    Properties.Settings.Default.Save();
                } else if (b.message.content.StartsWith("!RoleID"))
                {
                    foreach (var role in b.message.author.roles)
                    {
                        string id;
                        id = role.id;
                        client.SendMessageToChannel("Role id: " + id, dc);
                    }


                } else if (b.message.content.StartsWith("!WR"))
                {
                    ArrayList categories = new ArrayList();
                    string input;
                    input = b.message.content.Substring(4);
                    var game = SRC.Games.SearchGame(name: input);
                    foreach (var category in game.Categories)
                    {
                        categories.Add(category.Name);
                    }
                    var c = game.Categories.First(category => category.Name == "Any%" || category.Name == "100%" || category.Name == "Any% (No WW, DBO)");
                    var wr = c.WorldRecord;
                    var wrp = c.WorldRecord.Player;
                    client.SendMessageToChannel(input + " WR is " + wr + " by " + wrp , dc);


                }

            };
        }
        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //for debugging new features

        }
        //start timer interval setting here
        static double GetInterval() //30 min shower thought
        {
            //30 minute timer interval
            DateTime now = DateTime.Now;
            return ((1800 - now.Second) * 1000 - now.Millisecond);
        }
        static double GetInterval1() //hourly gif
        {
            //hourly timer interval
            DateTime now = DateTime.Now;
            return ((3600 - now.Second) * 1000 - now.Millisecond);
        }

        static double GetInterval2() //random meme
        {
            //5m-60m timer interval
            DateTime now = DateTime.Now;
            Random rnd = new Random();
            int r = rnd.Next(300, 3600);
            return ((r - now.Second) * 1000 - now.Millisecond);
        }
        static double GetInterval3() //Check for live streams
        {
            //5 minute interval
            DateTime now = DateTime.Now;
            return ((300 - now.Second) * 1000 - now.Millisecond);
        }
        //end timer intervals setting and start on timer elapsed events
        public void tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e) 
        {
            //Shower thought (30 mins)
            client.SendMessageToChannel("Half hourly Shower thought:", dc);
            //reddit
            var reddit = new Reddit();
            var subreddit = reddit.GetSubreddit("/r/showerthoughts");
            foreach (var post in subreddit.New.Take(1))
            {
                string st;
                st = post.Title;
                monoFlat_TextBox2.Text += st;

                client.SendMessageToChannel(st, dc);
            }
            tmr.Interval = GetInterval();
            tmr.Start();
        }


        public void tmr1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Gif (1hr)
            client.SendMessageToChannel("Hourly gif:", dc);
            var reddit = new Reddit();
            string sub;
            sub = "/r/gifs";
            var subreddit = reddit.GetSubreddit(sub);
            monoFlat_TextBox2.Text += subreddit;
            foreach (var post in subreddit.New.Take(1))
            {
                string st;
                st = post.Url.ToString();

                monoFlat_TextBox2.Text += st;

                client.SendMessageToChannel(st, dc);
            }
            tmr1.Interval = GetInterval1();
            tmr1.Start();
        }


        public void tmr2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Meme (random 5-60mins)
            ArrayList list = new ArrayList();
            list.Add("Do the eggs kill you?");
            list.Add("I herd u liek mudkipzz");
            list.Add("For an easy of access");
            list.Add("margherita");
            list.Add("Fakie fakie");
            list.Add("Whats a boshy");
            list.Add("did solgryn made this?");
            list.Add("Boshy 2 is best speedgame");
            list.Add("Legless Egg");
            list.Add("Whens boshy 3?");
            list.Add("3 2 1 2");
            Random rnd = new Random();
            int r = rnd.Next(list.Count);
            client.SendMessageToChannel((string)list[r], dc);
            tmr2.Interval = GetInterval2();
            tmr2.Start();
        }


        public void tmr3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //////////////////
            //  TWITCH API  // //This could get messy
            //////////////////

         //   clientid = "cj8ikwwt64wxo4thtttja5y9lhy0j8m";

         //   foreach (Stream channel1 in Twient.GetFollowedStreams().List)
       //     {
        //        string strm;
       //         string title;
       //         strm = channel1.Channel.DisplayName.ToString();
         //       title = channel1.Channel.Status.ToString();
        //        client.SendMessageToChannel("Currently Live: " + strm + title +"\r\n", dc);
        //    }
      //      tmr3.Interval = GetInterval3();
      //      tmr3.Start();
        }

        public void OnProcessExit(object sender, EventArgs e)
        {
            client.SendMessageToChannel("Cya nerds o/", dc);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //for exit message
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            //Declare timers
            tmr = new System.Timers.Timer();
            tmr.AutoReset = false;
            tmr.Elapsed += new System.Timers.ElapsedEventHandler(tmr_Elapsed);
            tmr.Interval = GetInterval();
            tmr.Start();

            tmr1 = new System.Timers.Timer();
            tmr1.AutoReset = false;
            tmr1.Elapsed += new System.Timers.ElapsedEventHandler(tmr1_Elapsed);
            tmr1.Interval = GetInterval1();
            tmr1.Start();

            tmr2 = new System.Timers.Timer();
            tmr2.AutoReset = false;
            tmr2.Elapsed += new System.Timers.ElapsedEventHandler(tmr2_Elapsed);
            tmr2.Interval = GetInterval2();
            tmr2.Start();

            tmr3 = new System.Timers.Timer();
            tmr3.AutoReset = false;
            tmr3.Elapsed += new System.Timers.ElapsedEventHandler(tmr3_Elapsed);
            tmr3.Interval = GetInterval3();
            tmr3.Start();


            //Discord Roles//
            //    Maybe    //
            admin.id = ""; //role id here, admin is a DiscordRole object




            //List Stuff//
            //Oh lord help me//
            list = Properties.Settings.Default.MySetting;
            Properties.Settings.Default.MySetting = list; //tbh idk why this works
            

             //   list.Add("Do the eggs kill you?");
          //    list.Add("I herd u liek mudkipzz");
          //     list.Add("For an easy of access");
          //     list.Add("margherita");
         //      list.Add("Fakie fakie");
          //       list.Add("Whats a boshy");
         //      list.Add("did solgryn made this?");
          //      list.Add("Boshy 2 is best speedgame");
           //      list.Add("Legless Egg");
           //      list.Add("Whens boshy 3?");
           //      list.Add("3 2 1 2");
            Properties.Settings.Default.Save();
            //start client
            monoFlat_TextBox2.Text += "Client Created \r\n";
            monoFlat_TextBox2.Text += "Assigned values variables \r\n";
            client.ClientPrivateInformation.email = ""; //Discord Email
            client.ClientPrivateInformation.password = ""; //Discord Password
            monoFlat_TextBox2.Text += "Assigned variables to ClientPrivateInfo \r\n";
            client.Connected += (sender1, a) =>
            {
                monoFlat_TextBox2.Text += ($"Connected! User: {a.user.user.username} \r\n");
            };
            monoFlat_TextBox2.Text += "Sending Login credidentials \r\n";
            try
            {
                client.SendLoginRequest(); //login
            }
            catch (Exception err) when (err is NullReferenceException || err is System.Net.WebException)
            {
                monoFlat_TextBox2.Text += err.Message; //catch exceptions for login, servers down etc etc.
            }

            Thread t = new Thread(client.Connect);
            monoFlat_TextBox2.Text += "Thread created...";
            t.Start();
            monoFlat_TextBox2.Text += "Thread started \r\n";
            monoFlat_TextBox2.Text += "Client Connected \r\n";
            Thread thread = new Thread(new ThreadStart(commands));
            thread.Start();
            timer1.Start();
            ds.id = ""; //discord server id
            dc.id = ""; //discord channel id
            client.SendMessageToChannel("Bot finished booting", dc);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Updates Uptime label
            monoFlat_Label2.Text = sw.Elapsed.ToString();
            monoFlat_Label2.Refresh();
        }

        private void monoFlat_Button1_Click(object sender, EventArgs e)
        {
            //Update Bots Game
            client.UpdateCurrentGame(monoFlat_TextBox1.Text);
        }


        private void monoFlat_Button2_Click(object sender, EventArgs e)
        {
            client.SendMessageToChannel(monoFlat_TextBox3.Text, dc);
        }
    }
}
