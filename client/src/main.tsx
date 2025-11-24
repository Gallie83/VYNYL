import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router'
import './index.css'
import App from './App.tsx'
import HomePage from './pages/HomePage.tsx'
import { AuthProvider } from './contexts/AuthContext/AuthContext.tsx'
import { AlbumProvider } from './contexts/AlbumContext/AlbumContext.tsx'
import { GroupProvider } from './contexts/GroupContext/GroupContext.tsx'
import AlbumInfo from './pages/AlbumInfo.tsx'

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      {
        index: true,
        element: <HomePage />
      },
      {
        path: '/album-info/:artistName/:albumName',
        element: <AlbumInfo />
      }
    ]
  }
])

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AuthProvider>
      <AlbumProvider>
        <GroupProvider>
          <RouterProvider router={router} />
        </GroupProvider>
      </AlbumProvider>
    </AuthProvider>
  </StrictMode>,
)
