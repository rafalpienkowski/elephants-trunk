#!/usr/bin/env bash

git log --pretty=format:'[%h] %aN %ad %s' --date=short --numstat --no-merges --after=2020-01-01 -- '*.cs' > revisions.log

Trunk.App dimensions measure lines-of-code .
Trunk.App dimensions measure lines-added-by-author revisions.log

Trunk.App analyze knowledge-map lines-added-by-author.csv

Trunk.App visualize d3-knowledgemap knowledge-nodes.csv
