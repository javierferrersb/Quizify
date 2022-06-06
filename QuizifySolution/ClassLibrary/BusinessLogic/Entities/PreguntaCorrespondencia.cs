using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PreguntaCorrespondencia
    {
        public PreguntaCorrespondencia()
        {
            CombosCorrespondencia = new List<ComboCorrespondencia>();
        }

        public PreguntaCorrespondencia ( string enunciado, Instructor autor, BateriaPreguntas bateria) : base( enunciado, autor, bateria)
        {
            CombosCorrespondencia = new List<ComboCorrespondencia>();
            AddAnswer();
        }

        public List<string> GetFrases()
        {
            List<string> frases = new List<string>();
            foreach(ComboCorrespondencia c in CombosCorrespondencia)
            {
                frases.Add(c.Frase);
            }
            return frases;
        }

        public void SetFrases(string[] frases)
        {
            for(int i = 0; i < CombosCorrespondencia.Count; i++)
            {
                CombosCorrespondencia.ElementAt(i).Frase = frases[i];
            }
        }

        public List<string> GetTerminos()
        {
            List<string> terminos = new List<string>();
            foreach (ComboCorrespondencia c in CombosCorrespondencia)
            {
                terminos.Add(c.Termino);
            }
            return terminos;
        }

        public void SetTerminos(string[] terminos)
        {
            for (int i = 0; i < CombosCorrespondencia.Count; i++)
            {
                CombosCorrespondencia.ElementAt(i).Termino = terminos[i];
            }
        }

        public void initPreguntaCorrespondencia(string frase, string termino)
        {
            RespuestaCorrectaCorrespondencia rpc = (RespuestaCorrectaCorrespondencia)respuestaCorrecta;
            for(int i = 0; i < 5; i++)
            {
                rpc.ParesCorrectosTerminoFrase.Add(i, i);
                ComboCorrespondencia combo = new ComboCorrespondencia(termino, frase, this);
                CombosCorrespondencia.Add(combo);
            }
        }

        public void changeNumFrases(int numeroFrases)
        {
            CombosCorrespondencia = new List<ComboCorrespondencia>();
            RespuestaCorrectaCorrespondencia rpc = (RespuestaCorrectaCorrespondencia)respuestaCorrecta;
            rpc.ParesCorrectosTerminoFrase = new Dictionary<int, int>();
            for (int i = 0; i < numeroFrases; i++)
            {
                CombosCorrespondencia.Add(new ComboCorrespondencia());
                rpc.ParesCorrectosTerminoFrase.Add(i, i);
            }
        }

        public override RespuestaCorrecta CreateCorrectAnswer()
        {
            return new RespuestaCorrectaCorrespondencia();
        }

        public override Pregunta CopyQuestion()
        {
            PreguntaCorrespondencia pregunta = new PreguntaCorrespondencia();
            pregunta.SetStatement(this.GetStatement());
            pregunta.Autor = this.Autor;
            ComboCorrespondencia comboNuevo;
            foreach(ComboCorrespondencia combo in this.CombosCorrespondencia)
            {
                comboNuevo = new ComboCorrespondencia();
                comboNuevo.Frase = combo.Frase;
                comboNuevo.Termino = combo.Termino;
                pregunta.CombosCorrespondencia.Add(comboNuevo);
            }
            pregunta.respuestaCorrecta = respuestaCorrecta.CopyRespuestaCorrecta();
            return pregunta;
        }
    }
}
