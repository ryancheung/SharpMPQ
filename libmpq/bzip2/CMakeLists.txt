# MIT License - Copyright (C) ryancheung
# This file is subject to the terms and conditions defined in
# file 'LICENSE', which is part of this source code package.

file(GLOB sources *.c)
add_library(bzip2 STATIC
  ${sources})

set_target_properties(bzip2 PROPERTIES LINKER_LANGUAGE CXX)

target_include_directories(bzip2
  PUBLIC
    ${CMAKE_CURRENT_SOURCE_DIR})

target_link_libraries(bzip2
  PRIVATE
    dependency-interface)
