# MIT License - Copyright (C) ryancheung
# This file is subject to the terms and conditions defined in
# file 'LICENSE', which is part of this source code package.

SET(zlib_STAT_SRCS
  adler32.c
  compress.c
  crc32.c
  deflate.c
  infback.c
  inffast.c
  inflate.c
  inftrees.c
  trees.c
  uncompr.c
  zutil.c
)

add_library(zlib STATIC
  ${zlib_STAT_SRCS})

set_target_properties(zlib PROPERTIES LINKER_LANGUAGE CXX)

target_include_directories(zlib
  PUBLIC
    ${CMAKE_CURRENT_SOURCE_DIR})

target_link_libraries(zlib
  PRIVATE
    dependency-interface)
