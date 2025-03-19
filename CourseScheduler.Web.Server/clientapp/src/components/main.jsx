import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import '../css/index.css'
import SchedulerApp from './SchedulerApp.jsx'
import { AppProvider } from "../context/AppContext";

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <AppProvider>
            <SchedulerApp />
        </AppProvider>
  </StrictMode>,
)
