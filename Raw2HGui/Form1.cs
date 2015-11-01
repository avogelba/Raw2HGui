using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows;
using System.IO;

namespace Raw2HGui
{
    public partial class Form1 : Form
    {
        private byte[] mBuffer;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Raw2H V1.0";
            button1.Text = "Convert...";
            textBox1.Font = new Font ("Arial" , 15);
            //textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //textBox1.Height = textBox1.PreferredHeight;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();//@"c:\";
            openFileDialog1.Title = "Open File...";
            openFileDialog1.Filter = "Minimal files (*.min)|*.min|Raw files (*.raw)|*.raw|Binary File (*.bin)|*.bin|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                        {
                            mBuffer = new byte[fs.Length];
                            fs.Read(mBuffer, 0, (int)fs.Length);
                            fs.Close();
                        }
                    }
                   
                    bool result = openFileDialog1.FileName.Equals(Path.ChangeExtension(openFileDialog1.FileName, ".h"),StringComparison.Ordinal);
                    if (!result)  {
                    using (StreamWriter outfile = new StreamWriter(Path.ChangeExtension(openFileDialog1.FileName, ".h")))
                    {
                        outfile.WriteLine(@"/* This file is generated with Raw2h for Windows*/");
                        outfile.WriteLine();
                        outfile.WriteLine(@"#ifndef _project_");
                        outfile.WriteLine(@"#define _project_");
                        outfile.WriteLine();
                        outfile.WriteLine(@"unsigned char raw_project[] =");
                        outfile.WriteLine(@"{");
                        outfile.Write(@"  ");
                        int i = 0;int buflen=0 ;
                        StringBuilder tempStr = new StringBuilder();
                        for (int j = 0; j < mBuffer.Length; j++)
                        {
                            tempStr.Append(mBuffer[j].ToString());
                            tempStr.Append(",");
                            outfile.Write(mBuffer[j]);
                            buflen = tempStr.Length;
                            outfile.Write(",");
                            //i++;
                            //if (i >= 32 || buflen >= 70) {
                            if (buflen >= 70)
                            {
                                tempStr.Clear();
                                outfile.WriteLine();
                                outfile.Write(@"  ");
                                i = 0;
                            }
                      
                        }
                        outfile.WriteLine();
                        outfile.WriteLine(@"};");
                        outfile.WriteLine();
                        outfile.Write(@"int raw_project_size=");
                        outfile.Write(mBuffer.Length);
                        outfile.WriteLine(@";");
                        outfile.WriteLine();
                        outfile.Write(@"/* Total ");
                        outfile.Write(mBuffer.Length);
                        outfile.WriteLine(@"bytes */");
                        outfile.WriteLine();
                        outfile.WriteLine(@"#endif");
                        outfile.WriteLine();
                        outfile.Flush();
                        outfile.Close();
                        textBox1.Text = "Sucessfully created file: " + Path.GetFileName(Path.ChangeExtension(openFileDialog1.FileName, ".h"));

                    }
                }
                else{
                    MessageBox.Show("Bad file selected!",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);

                        //MessageBoxIcon.Warning // for Warning  
                        //MessageBoxIcon.Error // for Error 
                        //MessageBoxIcon.Information  // for Information
                        //MessageBoxIcon.Question // for Question
                        //MessageBoxIcon.Asterisk //For Info Asterisk
                        //MessageBoxIcon.Exclamation //For triangle Warning 
                }

            }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            
            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created for Conspiracy's Source of ChaosTheory\nOriginal Demo can be found on:\n"+@"http://chaostheory.conspiracy.hu/"+"\n\nA. Vogelbacher 2015",
                                    "About",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
            
        }
    }
}

 