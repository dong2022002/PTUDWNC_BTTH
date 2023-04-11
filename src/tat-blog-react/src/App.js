import "./App.css";
import Footer from "./components/Footer";
import Index from "./Pages/Index";
import Layout from "./Pages/Layout";
import About from "./Pages/About";
import Contact from "./Pages/Contact";
import Rss from "./Pages/Rss";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import PostDetail from "./Pages/PostDetail";
import AdminLayout from './Pages/Admin/Layout'
import * as AdminIndex from './Pages/Admin/Index'
import Authors from './Pages/Admin/Authors'
import Categories from './Pages/Admin/Categories'
import Comments from './Pages/Admin/Comments'
import Posts from './Pages/Admin/Post/Post'
import Tags from './Pages/Admin/Tags'
import NotFound from "./Pages/NotFound";
import BadRequest from "./Pages/BadRequest";
function App() {
  return (
    <div>
      <Router>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route path="/" element={<Index />} />
            <Route path="blog" element={<Index />} />
            <Route path="blog/post" element={<PostDetail />} />
            <Route path="blog/about" element={<About />} />
            <Route path="blog/contact" element={<Contact />} />
            <Route path="blog/RSS" element={<Rss />} />
          </Route>
          <Route path="/admin" element={<AdminLayout />}>
            <Route path="/admin" element={<AdminIndex.default />} />
            <Route path="/admin/authors" element={<Authors />} />
            <Route path="/admin/categories" element={<Categories />} />
            <Route path="/admin/comments" element={<Comments />} />
            <Route path="/admin/posts" element={<Posts />} />
            <Route path="/admin/tags" element={<Tags />} />
          </Route>
          <Route path="/400" element={<BadRequest />}></Route>
          <Route path="*" element={<NotFound />}></Route>
        </Routes>
        <Footer />
      </Router>
    </div>
  );
}

export default App;
