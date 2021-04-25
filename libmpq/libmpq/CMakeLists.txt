# MIT License - Copyright (C) ryancheung
# This file is subject to the terms and conditions defined in
# file 'LICENSE', which is part of this source code package.

file(GLOB sources libmpq/*.c libmpq/*.h)

add_library(mpq SHARED ${sources})

set_target_properties(mpq PROPERTIES LINKER_LANGUAGE CXX)

if(WIN32)
  set(WIN_EXTRA_INCLUDE ${CMAKE_CURRENT_SOURCE_DIR}/win)
endif()

target_include_directories(mpq
  PUBLIC
    ${CMAKE_CURRENT_SOURCE_DIR}
    ${WIN_EXTRA_INCLUDE}
    ${CMAKE_SOURCE_DIR}/zlib
    ${CMAKE_SOURCE_DIR}/bzip2)

target_link_libraries(mpq
  PRIVATE
    dependency-interface
  PUBLIC
    zlib
    bzip2)
