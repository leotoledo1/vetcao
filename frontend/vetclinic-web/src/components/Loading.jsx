import { LoaderCircle } from 'lucide-react'

// Estado simples de carregamento exibido enquanto a API responde.
function Loading({ label = 'Carregando dados...' }) {
  return (
    <div className="loading-state">
      <LoaderCircle size={20} className="loading-icon" />
      <span>{label}</span>
    </div>
  )
}

export default Loading
