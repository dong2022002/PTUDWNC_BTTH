import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import "./App.css";
import Footer from "./components/Footer";
import About from "./Pages/About";
import Authors from "./Pages/Admin/Authors";
import Categories from "./Pages/Admin/Categories";
import Comments from "./Pages/Admin/Comments";
import * as AdminIndex from "./Pages/Admin/Index";
import AdminLayout from "./Pages/Admin/Layout";
import Edit from "./Pages/Admin/Post/Edit";
import Posts from "./Pages/Admin/Post/Post";
import Tags from "./Pages/Admin/Tags";
import BadRequest from "./Pages/BadRequest";
import Contact from "./Pages/Contact";
import Index from "./Pages/Index";
import Layout from "./Pages/Layout";
import NotFound from "./Pages/NotFound";
import PostDetail from "./Pages/PostDetail";
import Rss from "./Pages/Rss";
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
            <Route path="/admin/posts/edit" element={<Edit />} />
            <Route path="/admin/posts/edit/:id" element={<Edit />} />
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
