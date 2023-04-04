/* eslint-disable no-unused-vars */
import './App.css'
import Navbar from './components/Navbar'
import Sidebar from './components/Sidebar'
import Footer from './components/Footer'
import Index from './Pages/Index'
import Layout from './Pages/Layout'
import About from './Pages/About'
import Contact from './Pages/Contact'
import Rss from './Pages/Rss'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
function App() {
  return (
    <div>
      <Router>
        <Navbar />
        <div className='container-fluid'>
          <div className='row'>
            <div className='col-9'>
              <Routes>
                <Route path='/' element={<Layout />}>
                  <Route path='' element={<Index />} />
                  <Route path='blog' element={<Index />} />
                  <Route path='blog/about' element={<About />} />
                  <Route path='blog/contact' element={<Contact />} />
                  <Route path='blog/RSS' element={<Rss />} />
                </Route>
              </Routes>
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
