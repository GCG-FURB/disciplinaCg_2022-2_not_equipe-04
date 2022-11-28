/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;
using Accord.Math;


namespace gcgcg
{
    class Mundo : GameWindow
    {
        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height)
        {
            if (instanciaMundo == null)
                instanciaMundo = new Mundo(width, height);
            return instanciaMundo;
        }

        private CameraOrtho camera = new CameraOrtho();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private char objetoId;
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;
        private Poligono objetoNovo = null;
        private bool IsMovingVertice = false;
        private Ponto4D pontoMaisProximo = null;
        private Ponto4D rastroPoligono;
        private SegReta rastroPontoInicialDoPoligno = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = 0; camera.xmax = 600; camera.ymin = 0; camera.ymax = 600;

            SegReta segReta1 = new SegReta('x', null, new Ponto4D(300, 300), new Ponto4D(500, 300))
            {
                ObjetoCor = new Cor(255, 0, 0)
            };

            SegReta segReta2 = new SegReta('y', null, new Ponto4D(300, 300), new Ponto4D(300, 500))
            {
                ObjetoCor = new Cor(0, 255, 0)
            };

            GL.ClearColor(new Color(53, 56, 57, 2));
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
#if CG_Gizmo
            Sru3D();
#endif
            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();
            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.H)
                Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape)
                Exit();
            else if (e.Key == Key.E)
            {
                Console.WriteLine("--- Objetos / Pontos: ");
                for (var i = 0; i < objetosLista.Count; i++)
                {
                    Console.WriteLine(objetosLista[i]);
                }
            }
            else if (e.Key == Key.Enter)
            {
                objetosLista.Remove(rastroPontoInicialDoPoligno);

                if (objetoNovo != null)
                {
                    objetoNovo.PontosRemoverUltimo();   
                    objetoNovo.PontosAdicionar(objetoNovo.pontosLista[0]);
                    objetoSelecionado = objetoNovo;


                    objetoNovo = null;
                }
                rastroPoligono = null;
            }
            else if (e.Key == Key.Space)
            {
                DesenhaPoligono();
            }
        }

        private void DesenhaPoligono()
        {
            if (IsMovingVertice)
                return;

            if (objetoNovo == null)
            {

                objetoId = Utilitario.charProximo(objetoId);
                objetoNovo = new Poligono(objetoId, objetoSelecionado);

                objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
                rastroPoligono = new Ponto4D(mouseX, mouseY); 
                rastroPontoInicialDoPoligno = new SegReta(objetoId, null, new Ponto4D(mouseX, mouseY), rastroPoligono);
                objetoNovo.PontosAdicionar(rastroPoligono);

                if (null == objetoSelecionado)
                    objetosLista.Add(objetoNovo);

                objetosLista.Add(rastroPontoInicialDoPoligno);
            }
            else
            {
                objetoNovo.PontosRemoverUltimo();
                objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
                objetoNovo.PontosAdicionar(rastroPoligono);
            }
        }

    
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; 

            if (rastroPoligono != null && objetoNovo != null)
            {
                rastroPoligono.X = mouseX;
                rastroPoligono.Y = mouseY;
            }

            if (IsMovingVertice)
            {
                MoveVertice();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; 


            if (objetoSelecionado != null && IsMovingVertice)
            {
                MoveVertice();
                IsMovingVertice = true;
            }
            else if (!IsMovingVertice)
            {
                DesenhaPoligono();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            IsMovingVertice = false;
            pontoMaisProximo = null;
        }

        private void MoveVertice()
        {
            double distanciaMaisProxima = 999999;

            foreach (var ponto in objetoSelecionado.pontosLista)
            {
                var euclides = Distance.Euclidean(ponto.X, ponto.Y, mouseX, mouseY);

                if (euclides < distanciaMaisProxima)
                {
                    pontoMaisProximo = ponto;
                    distanciaMaisProxima = euclides;
                }
            }

            if (IsMovingVertice)
            {
                pontoMaisProximo.X = mouseX;
                pontoMaisProximo.Y = mouseY;
                return;
            }
        }

#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            // GL.Color3(1.0f,0.0f,0.0f);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            // GL.Color3(0.0f,1.0f,0.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            // GL.Color3(0.0f,0.0f,1.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();

            //GL.LineWidth(1);
            //GL.Begin(PrimitiveType.Lines);
            //GL.Color3(255, 0, 0);
            //GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            //GL.Color3(0, 255, 0);
            //GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            //GL.Color3(0, 0, 255);
            //GL.End();
        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2";
            window.Run(1.0 / 60.0);
        }
    }
}
