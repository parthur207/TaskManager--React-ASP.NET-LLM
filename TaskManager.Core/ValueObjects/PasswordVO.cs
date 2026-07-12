namespace TaskManager.Core.ValueObjects
{
    public class PasswordVO
    {

        protected PasswordVO() { }
        public string Value { get; }

        public PasswordVO(string password, bool isLogin=false, bool isResetPassword=false)
        {
            if (isLogin)
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("A senha não pode ser vazia.");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    if (isResetPassword)
                    {
                        throw new ArgumentException("A nova senha não pode ser vazia.");
                    }
                    else
                    {
                        throw new ArgumentException("A senha não pode ser vazia.");
                    }
                }
                if (password.Length < 6)
                {
                    if (isResetPassword)
                    {
                        throw new ArgumentException("A nova senha deve conter no mínimo 6 caracteres.");
                    }
                    else
                    {
                        throw new ArgumentException("A senha deve conter no mínimo 6 caracteres.");
                    }
                }

                if (!password.Any(char.IsUpper)) 
                {
                    if (isResetPassword)
                    {
                        throw new ArgumentException("A nova senha deve conter ao menos uma letra maiúscula.");
                    }
                    else
                    {
                        throw new ArgumentException("A  senha deve conter ao menos uma letra maiúscula.");
                    }
                }

                if (!password.Any(IsSpecialCharacter))
                {
                    if (isResetPassword)
                    {
                        throw new ArgumentException("A nova senha deve conter ao menos um caractere especial.");
                    }
                    else
                    {
                        throw new ArgumentException("A senha deve conter ao menos um caractere especial.");
                    }
                }
            }

            Value = password;
        }

        private bool IsSpecialCharacter(char c)
        {
            return !char.IsLetterOrDigit(c);
        }
    }
}
