                                                  
function StatusBadge({ status }) {
  const normalizedStatus = (status || '').toLowerCase()
  const className =
    normalizedStatus === 'agendada'
      ? 'status-badge scheduled'
      : normalizedStatus === 'cancelada'
        ? 'status-badge cancelled'
        : normalizedStatus === 'realizada'
          ? 'status-badge done'
          : 'status-badge'

  return <span className={className}>{status || 'Sem status'}</span>
}

export default StatusBadge
