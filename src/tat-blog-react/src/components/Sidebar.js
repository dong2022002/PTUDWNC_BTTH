import React from 'react'
import SearchForm from './SearchForm'
import CategoriesWidget from './CategoriesWidget'
import FeaturedPost from './FeaturedPost'

const Sidebar = () => {
  return (
    <div className='pt-4 ps-2'>
      <SearchForm />
      <CategoriesWidget/>
      <FeaturedPost/>
      <h1>Đăng ký nhận tin mới</h1>
      <h1>Tag Cloud</h1>
    </div>
  )
}

export default Sidebar
