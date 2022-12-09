
#define CG_Gizmo

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;
using CG_N2;
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
        private string objetoId = "0";
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;
        //private bool mouseMoverPto = false;
        private Poligono objetoNovo = null;
        private bool estahMudandoVertice = false;
        private Ponto4D pontoMaisProximo = null;
        private Ponto4D rastroPoligono;
        private Ponto4D backupUltimoPontoPoligonoSelecionado = null;
        private SegReta rastroPontoInicialDoPoligno = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = 0; camera.xmax = 600; camera.ymin = 0; camera.ymax = 600;

            SegReta segReta1 = new SegReta("Reta X", null, new Ponto4D(300, 300), new Ponto4D(500, 300))
            {
                ObjetoCor = new Cor(255, 0, 0)
            };

            SegReta segReta2 = new SegReta("Reta Y", null, new Ponto4D(300, 300), new Ponto4D(300, 500))
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
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            else if (e.Key == Key.A)
                objetoSelecionado = ScanLine();
            else if (e.Key == Key.V)
            {
                estahMudandoVertice = true;
            }
            else if (e.Key == Key.D)
            {
                MovimentaVerticeDoObjetoMaisProximo();
                if (pontoMaisProximo != null)
                {
                    objetoSelecionado.pontosLista.Remove(pontoMaisProximo);
                    estahMudandoVertice = false;
                }
            }
            else if (e.Key == Key.Enter)
            {
                //objetosLista.Remove(rastroPontoInicialDoPoligno);

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
                DesenhaNovoPoligonoComMouseOuEspaco();
            }
<<<<<<< HEAD
            else if (objetoSelecionado != null)
            {
                if (e.Key == Key.M)
                    Console.WriteLine(objetoSelecionado.Matriz);
                else if (e.Key == Key.P)
                    Console.WriteLine(objetoSelecionado);
                else if (e.Key == Key.C)
                {
                    if (objetoSelecionado.ObjetoPai != null)
                        objetoSelecionado.ObjetoPai.FilhoRemover(objetoSelecionado);
                    else
                        objetosLista.Remove(objetoSelecionado);

                    objetoSelecionado = null;
                }
                else if (e.Key == Key.R)
                    objetoSelecionado.ObjetoCor = new Cor(255, 0, 0, 0);
                else if (e.Key == Key.G)
                    objetoSelecionado.ObjetoCor = new Cor(0, 255, 0, 0);
                else if (e.Key == Key.B)
                    objetoSelecionado.ObjetoCor = new Cor(0, 0, 255, 0);
                else if (e.Key == Key.S)
                {
                    if (backupUltimoPontoPoligonoSelecionado == null)
                    {
                        backupUltimoPontoPoligonoSelecionado = objetoSelecionado.PontosUltimo();
                        objetoSelecionado.PontosRemoverUltimo();
                    }
                    else
                    {
                        objetoSelecionado.PontosAdicionar(backupUltimoPontoPoligonoSelecionado);
                        backupUltimoPontoPoligonoSelecionado = null;
                    }
                }
                else if (e.Key == Key.Left)
                    objetoSelecionado.TranslacaoXYZ(-10, 0, 0);
                else if (e.Key == Key.Right)
                    objetoSelecionado.TranslacaoXYZ(10, 0, 0);
                else if (e.Key == Key.Up)
                    objetoSelecionado.TranslacaoXYZ(0, 10, 0);
                else if (e.Key == Key.Down)
                    objetoSelecionado.TranslacaoXYZ(0, -10, 0);
                else if (e.Key == Key.PageUp)
                    objetoSelecionado.EscalaXYZ(2, 2, 2);
                else if (e.Key == Key.PageDown)
                    objetoSelecionado.EscalaXYZ(0.5, 0.5, 0.5);
                else if (e.Key == Key.Home)
                    objetoSelecionado.EscalaXYZBBox(0.5, 0.5, 0.5);
                else if (e.Key == Key.End)
                    objetoSelecionado.EscalaXYZBBox(2, 2, 2);
                else if (e.Key == Key.Number1)
                    objetoSelecionado.Rotacao(10);
                else if (e.Key == Key.Number2)
                    objetoSelecionado.Rotacao(-10);
                else if (e.Key == Key.Number3)
                    objetoSelecionado.RotacaoZBBox(10);
                else if (e.Key == Key.Number4)
                    objetoSelecionado.RotacaoZBBox(-10);
                else if (e.Key == Key.Number9)
                    objetoSelecionado = null;                 
                else
                    Console.WriteLine(" Não implementado");
            }
            else
                Console.WriteLine("Não implementado.");
=======
            else if (e.Key == Key.C) {
                if (objetoSelecionado != null) {
                    objetosLista.Remove(objetoSelecionado);
                    objetoSelecionado = null;
                }
            }
>>>>>>> 04173a5ff1a01dc50fbc41371e11e261517a2f22
        }

        private void DesenhaNovoPoligonoComMouseOuEspaco()
        {
            if (estahMudandoVertice)
                return;

            if (objetoNovo == null)
            {

                objetoId = Utilitario.charProximo(objetoId);
                objetoNovo = new Poligono(objetoId, objetoSelecionado);

                objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
                rastroPoligono = new Ponto4D(mouseX, mouseY); 
                rastroPontoInicialDoPoligno = new SegReta($"segReto_{objetoId}", null, new Ponto4D(mouseX, mouseY), rastroPoligono);
                objetoNovo.PontosAdicionar(rastroPoligono);

                if (objetoSelecionado == null)
                    objetosLista.Add(objetoNovo);

                // objetosLista.Add(rastroPontoInicialDoPoligno);
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

            if (estahMudandoVertice)
            {
                MovimentaVerticeDoObjetoMaisProximo();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; 


            if (objetoSelecionado != null && estahMudandoVertice)
            {
                MovimentaVerticeDoObjetoMaisProximo();
                estahMudandoVertice = true;
            }
<<<<<<< HEAD
            else if (!estahMudandoVertice)
            {
                DesenhaNovoPoligonoComMouseOuEspaco();
            }
=======
            // else if (!IsMovingVertice)
            // {
            //     DesenhaPoligono();
            // }
>>>>>>> 04173a5ff1a01dc50fbc41371e11e261517a2f22
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            estahMudandoVertice = false;
            pontoMaisProximo = null;
        }

        private void MovimentaVerticeDoObjetoMaisProximo()
        {
            double distanciaMaisProxima = 100000000;

            foreach (var ponto in objetoSelecionado.pontosLista)
            {
                var euclides = Distance.Euclidean(ponto.X, ponto.Y, mouseX, mouseY);

                if (euclides < distanciaMaisProxima)
                {
                    pontoMaisProximo = ponto;
                    distanciaMaisProxima = euclides;
                }
            }

            if (estahMudandoVertice)
            {
                pontoMaisProximo.X = mouseX;
                pontoMaisProximo.Y = mouseY;
                return;
            }
        }

        private Poligono ScanLine()
        {
            Poligono plg = null;

            List<Objeto> objetos = new List<Objeto>();


            objetosLista.ForEach(obj => objetos.AddRange(RetornarFilhos(obj)));

            foreach (Poligono objeto in objetos)
            {
                long qtdXises = 0;

                for (int i = 0; i < objeto.pontosLista.Count - 1; i++)
                {
                    Ponto4D p1 = objeto.pontosLista[i];
                    Ponto4D p2 = objeto.pontosLista[i + 1];

                    var teUm = (mouseY - p1.Y) / (p2.Y - p1.Y);
                    var xisUm = p1.X + (p2.X - p1.X) * teUm;

                    if (teUm <= 1 && teUm > 0 && xisUm >= mouseX)
                        qtdXises++;
                }

                if (qtdXises % 2 != 0)
                    plg = objeto;
            }

            return plg;
        }

        private List<Objeto> RetornarFilhos(Objeto objeto)
        {
            List<Objeto> filhos = new List<Objeto>();

            foreach (var obj in objeto.objetosLista)
                filhos.AddRange(RetornarFilhos(obj));

            filhos.Add(objeto);

            return filhos;
        }

#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N3";
            window.Run(1.0 / 60.0);
        }
    }
}
