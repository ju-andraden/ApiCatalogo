namespace APICatalogo.Services
{
    public class MeuServico : IMeuServico
    {
        public string Saudacao(string nome)
        {
           return $"Bem vinde, {nome} \n\n{DateTime.Now}";
        }
    }
}
