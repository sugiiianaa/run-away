#!/bin/bash
dotnet ef "$@" --project ./RunAway.Infrastructure --startup-project ./RunAway.API