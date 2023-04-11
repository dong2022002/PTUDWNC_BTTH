import React from 'react'
import {useQuery} from '../Utils/Utils';
const BadRequest = () => {
  let query = useQuery(),
    redirectTo = query.get('redirectTo') ?? '/'
  return (
    <div>BadRequest</div>
  )
}

export default BadRequest
