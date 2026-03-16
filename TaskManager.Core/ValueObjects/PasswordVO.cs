namespace TaskManager.Core.ValueObjects
{
    public class PasswordVO
    {
        public string Value { get; }

        public PasswordVO(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha não pode ser vazia.");

            if (password.Length < 6)
                throw new ArgumentException("A senha deve conter no mínimo 6 caracteres.");

            if (!password.Any(char.IsUpper))
                throw new ArgumentException("A senha deve conter ao menos uma letra maiúscula.");

            if (!password.Any(IsSpecialCharacter))
                throw new ArgumentException("A senha deve conter ao menos um caractere especial.");

            Value = password;
        }

        private bool IsSpecialCharacter(char c)
        {
            return !char.IsLetterOrDigit(c);
        }
    }
}
