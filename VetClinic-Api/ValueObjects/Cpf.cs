namespace VetClinic.ValueObjects
{
    // Value Object que normaliza e valida o CPF.
    public class Cpf
    {
        public string Value { get; private set; }

        public Cpf(string value)
        {
            var texto = value ?? string.Empty;
            var numeros = new string(texto.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(texto) || numeros.Length != 11)
            {
                throw new Exception("CPF com formato inválido");
            }

            Value = numeros;
        }
    }
}
