import React from 'react'
import SearchForm from './SearchForm'
import CategoriesWidget from './CategoriesWidget'
import FeaturedPost from './FeaturedPost'
import RandomPost from './RandomPost'
import TagWidgets from './TagWidgets'

const Sidebar = () => {
  return (
    <div className='pt-4 ps-2'>
      <SearchForm />
      <CategoriesWidget/>
      <FeaturedPost/>
      <RandomPost/>
      <TagWidgets />
    </div>
  )
}

export default Sidebar
