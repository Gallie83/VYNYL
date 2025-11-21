import { Outlet } from "react-router"

import Searchbar from "./components/Searchbar"

function App() {

  return (
    <>
      <Searchbar />
      <Outlet />
    </>
  )
}

export default App
