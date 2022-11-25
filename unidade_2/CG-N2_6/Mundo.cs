/**
  Autor: Dalton Solano dos Reis
**/

//#define CG_Privado // código do professor.
#define CG_Gizmo  // debugar gráfico.
#define CG_Debug // debugar texto.
#define CG_OpenGL // render OpenGL.
//#define CG_DirectX // render DirectX.

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;
using System.Threading;
using System.Threading.Tasks;

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
    private char objetoId = '@';
    private bool bBoxDesenhar = false;
    int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
    private bool mouseMoverPto = false;
    // private Retangulo obj_Retangulo;
    private SegReta obj_SegReta;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif
        private Ponto pontoSpline1;
        private Ponto pontoSpline2;
        private Ponto pontoSpline3;
        private Ponto pontoSpline4;
        private Ponto pontoSplineSelecionado;

        private Spline spline;      
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      camera.xmin = -300; camera.xmax = 300; camera.ymin = -300; camera.ymax = 300;

      DesenharSpline();
     

#if CG_Privado
      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta = new Privado_SegReta(objetoId, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      objetoId = Utilitario.charProximo(objetoId);
      obj_Circulo = new Privado_Circulo(objetoId, null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      obj_Circulo.PrimitivaTipo = PrimitiveType.Points;
      obj_Circulo.PrimitivaTamanho = 5;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
#if CG_OpenGL
      GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
#endif
    }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
#if CG_OpenGL
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
#endif
    }
    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
#if CG_OpenGL
      GL.Clear(ClearBufferMask.ColorBufferBit);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
#endif
#if CG_Gizmo      
      Sru3D();
#endif
      for (var i = 0; i < objetosLista.Count; i++)
        objetosLista[i].Desenhar();
#if CG_Gizmo
      if (bBoxDesenhar && (objetoSelecionado != null))
        objetoSelecionado.BBox.Desenhar();
#endif
      this.SwapBuffers();
    }

      long inicio = 100;


      private void DesenharSpline()
        {
            Ponto4D ponto1 = new Ponto4D(-200, -200);
            Ponto4D ponto2 = new Ponto4D(-200, 200);
            Ponto4D ponto3 = new Ponto4D(200, 200);
            Ponto4D ponto4 = new Ponto4D(200, -200);

            objetoId = Utilitario.charProximo(objetoId);
            pontoSpline1 = new Ponto(objetoId, null, ponto1);
            objetoId = Utilitario.charProximo(objetoId);
            pontoSpline2 = new Ponto(objetoId, null, ponto2);
            objetoId = Utilitario.charProximo(objetoId);
            pontoSpline3 = new Ponto(objetoId, null, ponto3);
            objetoId = Utilitario.charProximo(objetoId);
            pontoSpline4 = new Ponto(objetoId, null, ponto4);
            objetosLista.Add(pontoSpline1);
            objetosLista.Add(pontoSpline2);
            objetosLista.Add(pontoSpline3);
            objetosLista.Add(pontoSpline4);

            objetoId = Utilitario.charProximo(objetoId);
            obj_SegReta = new SegReta(objetoId, null,ponto1, ponto2);
            obj_SegReta.ObjetoCor.CorR = 0; obj_SegReta.ObjetoCor.CorG = 0; obj_SegReta.ObjetoCor.CorB = 255;
            objetosLista.Add(obj_SegReta);
            objetoSelecionado = obj_SegReta;

            objetoId = Utilitario.charProximo(objetoId);
            obj_SegReta = new SegReta(objetoId, null,ponto2, ponto3);
            obj_SegReta.ObjetoCor.CorR = 0; obj_SegReta.ObjetoCor.CorG = 0; obj_SegReta.ObjetoCor.CorB = 255;
            objetosLista.Add(obj_SegReta);
            objetoSelecionado = obj_SegReta;

            objetoId = Utilitario.charProximo(objetoId);
            obj_SegReta = new SegReta(objetoId, null,ponto3, ponto4);
            obj_SegReta.ObjetoCor.CorR = 0; obj_SegReta.ObjetoCor.CorG = 0; obj_SegReta.ObjetoCor.CorB = 255;
            objetosLista.Add(obj_SegReta);
            objetoSelecionado = obj_SegReta;

            objetoId = Utilitario.charProximo(objetoId);
            spline = new Spline(objetoId, null, ponto1, ponto2, ponto3, ponto4, 10);
            spline.PrimitivaTamanho = 5;
            spline.ObjetoCor.CorR = 255; spline.ObjetoCor.CorG = 255; spline.ObjetoCor.CorB = 0;
            objetosLista.Add(spline);
            objetoSelecionado = spline;

        }
    protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
    {

       if (e.Key == Key.H) {
         Utilitario.AjudaTeclado();
       }
       else if (e.Key == Key.M && spline != null) {
          spline.quantidadePontos++;
       }
       else if (e.Key == Key.N && spline != null)
            {
                if (spline.quantidadePontos > 1)
                    spline.quantidadePontos--;
            }
       else if (e.Key == Key.R && spline != null) {
            spline.quantidadePontos = 10;
       }
        else if (e.Key == Key.Number1 || e.Key == Key.Keypad1)
                SelecionarPonto(pontoSpline1);
            else if (e.Key == Key.Number2 || e.Key == Key.Keypad2)
                SelecionarPonto(pontoSpline2);
            else if (e.Key == Key.Number3 || e.Key == Key.Keypad3)
                SelecionarPonto(pontoSpline3);
            else if (e.Key == Key.Number4 || e.Key == Key.Keypad4)
                SelecionarPonto(pontoSpline4);
            else if (e.Key == Key.E)
                Esquerda();
            else if (e.Key == Key.D)
                Direita();
            else if (e.Key == Key.C)
            {
                Cima();
            }
            else if (e.Key == Key.B)
                Baixo();
            

      // else if (e.Key == Key.B) {
      //   camera.PanBaixo();
      // }
//       }
// #if CG_Gizmo
//       else if (e.Key == Key.O)
//         bBoxDesenhar = !bBoxDesenhar;
// #endif
//       else if (e.Key == Key.V)
//         mouseMoverPto = !mouseMoverPto;   //TODO: falta atualizar a BBox do objeto
//       else
//         Console.WriteLine(" __ Tecla não implementada.");
    }

      private void SelecionarPonto(Ponto ponto)
        {
            if (spline == null)
            {
                return;
            }
            pontoSpline1.ObjetoCor.CorR = 0; pontoSpline1.ObjetoCor.CorG = 0; pontoSpline1.ObjetoCor.CorB = 0;
            pontoSpline2.ObjetoCor.CorR = 0; pontoSpline2.ObjetoCor.CorG = 0; pontoSpline2.ObjetoCor.CorB = 0;
            pontoSpline3.ObjetoCor.CorR = 0; pontoSpline3.ObjetoCor.CorG = 0; pontoSpline3.ObjetoCor.CorB = 0;
            pontoSpline4.ObjetoCor.CorR = 0; pontoSpline4.ObjetoCor.CorG = 0; pontoSpline4.ObjetoCor.CorB = 0;
            ponto.ObjetoCor.CorR = 255; ponto.ObjetoCor.CorG = 0; ponto.ObjetoCor.CorB = 0;
            pontoSplineSelecionado = ponto;
        }

         private void Esquerda()
        {
            if (pontoSplineSelecionado != null)
            {
                pontoSplineSelecionado.ponto.X--;
                return;
            }
            camera.xmin--;
            camera.xmax--;
        }
        private void Direita()
        {
            if (pontoSplineSelecionado != null)
            {
                pontoSplineSelecionado.ponto.X++;
                return;
            }
            camera.xmin++;
            camera.xmax++;
        }

        private void Cima()
        {
            if (pontoSplineSelecionado != null)
            {
                pontoSplineSelecionado.ponto.Y++;
                return;
            }
            camera.ymin++;
            camera.ymax++;
        }

        private void Baixo()
        {
            if (pontoSplineSelecionado != null)
            {
                pontoSplineSelecionado.ponto.Y--;
                return;
            }
            camera.ymin--;
            camera.ymax--;
        }

    //TODO: não está considerando o NDC
    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
      mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
      if (mouseMoverPto && (objetoSelecionado != null))
      {
        objetoSelecionado.PontosUltimo().X = mouseX;
        objetoSelecionado.PontosUltimo().Y = mouseY;
      }
    }

#if CG_Gizmo
    private void Sru3D()
    {
#if CG_OpenGL
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
#endif
    }
#endif    
  }
  class Program
  {
    static void Main(string[] args)
    {
      Mundo window = Mundo.GetInstance(600, 600);
      window.Title = "CG_N2_1";
      window.Run(1.0 / 60.0);
    }
  }
}
