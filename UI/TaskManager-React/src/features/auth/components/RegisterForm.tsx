import { useRegister } from "../hooks/useRegister";

export default function RegisterForm() {
  const { form, handleChange, handleSubmit } = useRegister();

  return (
    <form onSubmit={handleSubmit}>
      <h1>Cadastro</h1>

      <input name="email" value={form.email} onChange={handleChange} />
      <input
        name="password"
        type="password"
        value={form.password}
        onChange={handleChange}
      />
      <input
        name="confirmPassword"
        type="password"
        value={form.confirmPassword}
        onChange={handleChange}
      />

      <button>Cadastrar</button>
    </form>
  );
}
