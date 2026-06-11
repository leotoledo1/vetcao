import { LoaderCircle } from 'lucide-react'

                                                                  
function Loading({ label = 'Carregando dados...' }) {
  return (
    <div className="loading-state">
      <LoaderCircle size={20} className="loading-icon" />
      <span>{label}</span>
    </div>
  )
}

export default Loading
