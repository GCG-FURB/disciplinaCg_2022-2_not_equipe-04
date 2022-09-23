using System;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;

namespace gcgcg 
{
    class Circulo : ObjetoGeometria 
    {
        public Circulo(char rotulo, Objeto paiRef, Ponto4D ptoCentro, double raio) : base(rotulo, paiRef)
        {
            base.PontosAdicionar(ptoCentro);
            double angulo = Matematica.GerarPtosCirculoSimetrico(raio);
            for (int i = 0; i < 72; i++)
            {
                base.PontosAdicionar(Matematica.GerarPtosCirculo(angulo, raio));
                angulo += 35;
            }
        }

        protected override void DesenharObjeto()
        {
            GL.Begin(base.PrimitivaTipo);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();
        }
    }
}