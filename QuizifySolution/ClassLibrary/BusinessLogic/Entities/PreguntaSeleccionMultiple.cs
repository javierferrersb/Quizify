using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PreguntaSeleccionMultiple
    {
        public PreguntaSeleccionMultiple()
        {
            CombosSeleccionMultiple = new List<ComboSeleccionMultiple>();
        }

        public PreguntaSeleccionMultiple(string enunciado, Instructor autor, BateriaPreguntas bateria) : base( enunciado, autor, bateria)
        {
            CombosSeleccionMultiple = new List<ComboSeleccionMultiple>();
            for(int i = 1; i <= 4; i++)
            {
                añadirOpcion("Option " + i);
            }
        }

        public override RespuestaCorrecta CreateCorrectAnswer()
        {
            return new RespuestaCorrectaMultiple();
        }

        public void añadirOpcion(string opcion)
        {
            CombosSeleccionMultiple.Add(new ComboSeleccionMultiple(opcion, false, this));
        }

        public override Pregunta CopyQuestion()
        {
            PreguntaSeleccionMultiple pregunta = new PreguntaSeleccionMultiple();
            pregunta.SetStatement(this.GetStatement());
            pregunta.Autor = this.Autor;
            foreach(ComboSeleccionMultiple combo in this.CombosSeleccionMultiple)
            {
                ComboSeleccionMultiple comboNuevo = new ComboSeleccionMultiple();
                comboNuevo.TextoRespuesta = combo.TextoRespuesta;
                comboNuevo.EsRespuestaCorrecta = combo.EsRespuestaCorrecta;
                comboNuevo.PreguntaCorrespondiente = pregunta;
                pregunta.CombosSeleccionMultiple.Add(comboNuevo);
            }
            return pregunta;
        }

    }
}