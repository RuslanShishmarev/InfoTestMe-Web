﻿using System;
using System.Collections.Generic;

namespace InfoTestMe.Common.Models
{
    public class CoursePageDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string AudioFileName { get; set; }
        public byte[] AudioFile { get; set; }
        public List<CourseBlockDTO> Blocks { get; set; }
    }
}
