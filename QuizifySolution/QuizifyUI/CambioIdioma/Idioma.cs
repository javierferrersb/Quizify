using System.Collections.Generic;
using System.IO;

namespace QuizifyUI
{
    public static class Idioma
    {
        public static Dictionary<string, string> info = new Dictionary<string, string>();
        public static string file;
        public static void cambiarIdioma(string archivo)
        {
            file = archivo;
            Properties.Settings config = new Properties.Settings();
            config.lang = archivo;
            config.Save();
            cargar(archivo);
        }
        private static void cargar(string archivo)
        {
            info.Clear();
            foreach (string linea in File.ReadLines("lang\\" + archivo))
            {
                if (linea.Contains("="))
                {
                    string[] s = linea.Split(new char[] { '=' });
                    info.Add(s[0], s[1]);
                }
            }
        }
        public static void actualizar()
        {

        }

        /* public static void Controles(Control p)
         {
             foreach (string control in info.Keys)
             {
                 try
                 {
                     /*w.Controls[control].Text = info[control];
                     w.Resources[control].Text = info[control];*/
        //Control c = (Control)p.Resources[control];
        /*   Control c = (Control)p.FindName(control);
           try
           {
               Button b = (Button) c;
               b.Content = info[control];
           }
           catch(Exception e6) {
               try
               {
                   Label l = (Label)c;
                   l.Content = info[control];
               }
               catch(Exception e7)
               {
                   try
                   {
                       RadioButton rb = (RadioButton)c;
                       rb.Content = info[control];
                   }
                   catch (Exception e2)
                   {
                       try
                       {
                           TextBox tbx = (TextBox)c;
                           tbx.Text = info[control];
                       }
                       catch (Exception e3)
                       {
                        /*   try
                           {
                               TextBlock tbl = (TextBlock)c;
                               tbl.Text = info[control];
                           }
                           catch (Exception e4)
                           {

                           }*/
        /*                      }
                          }
                      }
                  }
              }
              catch (Exception e5)
              {

              }
          }
      }*/
    }
}
