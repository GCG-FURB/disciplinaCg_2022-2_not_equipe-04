using CG_Biblioteca;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace gcgcg
{
    internal class Ponto : ObjetoGeometria {

        private Cor objetoCor = new Cor(0, 0, 0, 0);

        public Ponto4D ponto { get; set; }
        

        public Ponto(char rotulo, Objeto paiRef, Ponto4D ponto, int tamanho = 15) : base(rotulo, paiRef)
        {
            PrimitivaTamanho = tamanho;
            base.PrimitivaTipo = PrimitiveType.Points;
            base.PontosAdicionar(ponto);
            this.ponto = ponto;
        }

        protected override void DesenharObjeto()
        {
            GL.Begin(PrimitiveType.Points);
            GL.Color3(objetoCor.CorR, objetoCor.CorG, objetoCor.CorB);
            GL.Vertex2(pontosLista[0].X, pontosLista[0].Y);
            GL.End();
        }
    }
}