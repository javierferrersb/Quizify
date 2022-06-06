using Quizify.Services;
using Quizify.Entities;
using Quizify.Persistence;
using NUnit.Framework;
using System;

namespace TestProject1
{
    [TestFixture]
    public class QuizStatustests
    {
        Instructor instructor;
        Contenido contenido;

        [SetUp]
        public void Setup()
        {
            instructor = new Instructor("Patricio", "patricio@patricio.com", "contraseña");
            Curso curso = new("PSW", 1, "Proceso de Software", instructor);
            contenido = new Contenido("Cápsulas ágiles", curso);
        }

        [Test]
        public void QuizStartsWithBorrador()
        {
            Quiz quiz = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Assert.IsTrue(quiz.Estado is Borrador);
        }
        [Test]
        public void StatusAfterBorradorCancelado()
        {
            Quiz quiz = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz.Estado = BorradorCancelado.GetInstance();
            quiz.Lanzar();
            Assert.IsTrue(quiz.Estado is PublicadoInactivo);
        }
        [Test]
        public void StatusAfterBorrador()
        {
            Quiz quiz = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz.Lanzar();
            Assert.IsTrue(quiz.Estado is PublicadoInactivo);
        }
        [Test]
        public void StatusAfterPublicadoInactivo()
        {
            Quiz quiz = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz.Estado = PublicadoInactivo.GetInstance();
            quiz.Activar();
            Assert.IsTrue(quiz.Estado is PublicadoActivo);
        }
        [Test]
        public void StatusAfterPublicadoActivo()
        {
            Quiz quiz = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz.Estado = PublicadoActivo.GetInstance();
            quiz.Finalizar();
            Assert.IsTrue(quiz.Estado is Finalizado);
        }
        [Test]
        public void StatusAfterFinalizado()
        {
            Quiz quiz = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz.Estado = Finalizado.GetInstance();
            quiz.Calificar();
            Assert.IsTrue(quiz.Estado is Calificado);
        }
        [Test]
        public void StatusCancelPublicadoInactivo()
        {
            Quiz quiz = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz.Estado = PublicadoInactivo.GetInstance();
            quiz.Cancelar();
            Assert.IsTrue(quiz.Estado is BorradorCancelado);
        }
        [Test]
        public void BorradorAdmittedOperations()
        {
            Quiz quiz1 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz2 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz3 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz4 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz1.Estado = Borrador.GetInstance();
            quiz2.Estado = Borrador.GetInstance();
            quiz3.Estado = Borrador.GetInstance();
            quiz4.Estado = Borrador.GetInstance();
            quiz1.Activar();
            quiz2.Finalizar();
            quiz3.Cancelar();
            quiz4.Calificar();

            Assert.IsTrue(quiz1.Estado is Borrador &&
                          quiz2.Estado is Borrador &&
                          quiz3.Estado is Borrador &&
                          quiz4.Estado is Borrador);
        }
        [Test]
        public void BorradorCanceladoAdmittedOperations()
        {
            Quiz quiz1 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz2 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz3 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz4 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz1.Estado = BorradorCancelado.GetInstance();
            quiz2.Estado = BorradorCancelado.GetInstance();
            quiz3.Estado = BorradorCancelado.GetInstance();
            quiz4.Estado = BorradorCancelado.GetInstance();
            quiz1.Activar();
            quiz2.Finalizar();
            quiz3.Cancelar();
            quiz4.Calificar();

            Assert.IsTrue(quiz1.Estado is BorradorCancelado &&
                          quiz2.Estado is BorradorCancelado &&
                          quiz3.Estado is BorradorCancelado &&
                          quiz4.Estado is BorradorCancelado);
        }
        [Test]
        public void PublicadoInactivoAdmittedOperations()
        {
            Quiz quiz1 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz2 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz3 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz1.Estado = PublicadoInactivo.GetInstance();
            quiz2.Estado = PublicadoInactivo.GetInstance();
            quiz3.Estado = PublicadoInactivo.GetInstance();
            quiz1.Lanzar();
            quiz2.Finalizar();
            quiz3.Calificar();

            Assert.IsTrue(quiz1.Estado is PublicadoInactivo &&
                          quiz2.Estado is PublicadoInactivo &&
                          quiz3.Estado is PublicadoInactivo);
        }
        [Test]
        public void PublicadoActivoAdmittedOperations()
        {
            Quiz quiz1 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz2 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz3 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz4 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz1.Estado = PublicadoActivo.GetInstance();
            quiz2.Estado = PublicadoActivo.GetInstance();
            quiz3.Estado = PublicadoActivo.GetInstance();
            quiz4.Estado = PublicadoActivo.GetInstance();
            quiz1.Lanzar();
            quiz2.Activar();
            quiz3.Calificar();
            quiz4.Activar();

            Assert.IsTrue(quiz1.Estado is PublicadoActivo &&
                          quiz2.Estado is PublicadoActivo &&
                          quiz3.Estado is PublicadoActivo &&
                          quiz4.Estado is PublicadoActivo);
        }
        [Test]
        public void FinalizadoAdmittedOperations()
        {
            Quiz quiz1 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz2 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz3 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz4 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz1.Estado = Finalizado.GetInstance();
            quiz2.Estado = Finalizado.GetInstance();
            quiz3.Estado = Finalizado.GetInstance();
            quiz4.Estado = Finalizado.GetInstance();
            quiz1.Activar();
            quiz2.Finalizar();
            quiz3.Cancelar();
            quiz4.Lanzar();

            Assert.IsTrue(quiz1.Estado is Finalizado &&
                          quiz2.Estado is Finalizado &&
                          quiz3.Estado is Finalizado &&
                          quiz4.Estado is Finalizado);
        }
        [Test]
        public void CalificadoAdmittedOperations()
        {
            Quiz quiz1 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz2 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz3 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz4 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            Quiz quiz5 = new Quiz("Quiz 1", 0, 15, 30.0, DateTime.Now, instructor, 1, contenido, true, 1.0);
            quiz1.Estado = Calificado.GetInstance();
            quiz2.Estado = Calificado.GetInstance();
            quiz3.Estado = Calificado.GetInstance();
            quiz4.Estado = Calificado.GetInstance();
            quiz5.Estado = Calificado.GetInstance();
            quiz1.Lanzar();
            quiz2.Finalizar();
            quiz3.Cancelar();
            quiz4.Activar();
            quiz5.Calificar();
            Assert.IsTrue(quiz1.Estado is Calificado &&
                          quiz2.Estado is Calificado &&
                          quiz3.Estado is Calificado &&
                          quiz4.Estado is Calificado &&
                          quiz5.Estado is Calificado);
        }
    }
}
