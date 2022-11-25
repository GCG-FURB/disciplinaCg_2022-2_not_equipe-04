using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;

namespace gcgcg {
    class SegReta : ObjetoGeometria {

        public SegReta(char rotulo, Objeto paiRef, Ponto4D ptoIni, Ponto4D ptoFim) : base(rotulo, paiRef) {
            base.PontosAdicionar(ptoIni);
            base.PontosAdicionar(ptoFim);
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

        public override string ToString()
        {
            return base.ToString();
        }

        public Ponto4D PontosPosicao(int index)
        {
        return pontosLista[index];
        }
    }
}