function DashboardCard({ icon: Icon, label, value, accent, helper }) {
  return (
    <article className="dashboard-card" data-accent={accent}>
      <div className="dashboard-card-icon">
        <Icon size={22} />
      </div>
      <div className="dashboard-card-copy">
        <p>{label}</p>
        <strong>{value}</strong>
        <span>{helper}</span>
      </div>
    </article>
  )
}

export default DashboardCard
