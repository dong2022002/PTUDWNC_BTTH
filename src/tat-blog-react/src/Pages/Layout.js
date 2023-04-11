import Navbar  from "../components/Navbar";
import { Outlet } from "react-router-dom";
import Sidebar from "../components/Sidebar";

const Layout = () => {
  return (
    <>
      <Navbar />
      <div className="container-fluid py-3">
        <div className="row">
          <div className="col-9">
            <Outlet />
          </div>
          <div className="col-3 border-start">
            <Sidebar />
          </div>
        </div>
      </div>
    </>
  );
};

export default Layout;
