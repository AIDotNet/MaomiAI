import { create } from 'zustand'

interface AppState {
  count: number
  username: string
  teamId: string
  currentTeam: any
  increment: () => void
  decrement: () => void
  setUsername: (username: string) => void
  setTeamId: (teamId: string) => void
  setCurrentTeam: (team: any) => void
}

const useAppStore = create<AppState>((set) => ({
  count: 0,
  username: '',
  teamId: '',
  currentTeam: null,
  increment: () => set((state) => ({ count: state.count + 1 })),
  decrement: () => set((state) => ({ count: state.count - 1 })),
  setUsername: (username: string) => set({ username }),
  setTeamId: (teamId: string) => set({ teamId }),
  setCurrentTeam: (team: any) => set({ currentTeam: team }),
}))

export default useAppStore
