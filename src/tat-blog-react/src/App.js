/* eslint-disable no-unused-vars */
import './App.css'
import Navbar from './components/Navbar'
import Sidebar from './components/Sidebar'
import Footer from './components/Footer'
import Index from './Pages/Index'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
function App() {
  return (
    <div>
      <Router>
        <Navbar />
        <div className='container-fluid'>
          <div className='row'>
            <div className='col-9'>
              <Index />
            </div>
            <div className='col-3 boder-start'>
              <Sidebar />
            </div>
          </div>
        </div>
        <Footer />
      </Router>
    </div>
  )
}

export default App
